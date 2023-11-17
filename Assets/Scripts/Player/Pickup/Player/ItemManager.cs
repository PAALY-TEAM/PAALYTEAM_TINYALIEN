using System;
using System.Linq;
using Interfaces.ColourChange;
using Pickup.Crayon;
using Pickup.Shade;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using ColourHolder = Pickup.Shade.ColourHolder;


namespace Pickup.Player
{
    public class ItemManager : MonoBehaviour
    {
        [Header("Add gameObject to change the text of")]
        [SerializeField] private GameObject[] txt;
        //Number of collected objects
        public static int[] NumbCarried;
        //Number of stored objects in SpaceShip
        public static int[] NumbStored;
        [Header("Number of colours and which that the player can turn into")]
        public Material[] colours;
        //Renders of the different alien body parts so that they can change colour
        [Header("Attach all part of the alien that should change colour")]
        [SerializeField] private Renderer[] rend;
        //
        public event Action CrayonPickedUp;
        internal int CrayonProgress;
        //
        //The ID of the alien current colour
        [HideInInspector]
        public int currentColour = 0;
        //Objects that player needs information from (pos, scripts etc.)
        private GameObject _spaceShip;
        //To save the trigger (other) GameObject
        private GameObject _otherObject;
        //The player is in range to interact with NPC
        private bool _canInteract = false;
        //Text to make it easier for player to understand something
        public GameObject hintText;
        //Show the colour the player currently are using, because
        //the shapes are difficult to see on player
        private ShowColour _showColour;
        /// <summary>
        /// This part is to find all the objects in the scene by tag
        /// so that the different objects can change colour when the
        /// player picks up that colour
        /// The scripts continues in start
        /// </summary>
        private GameObject[][] _objectsToChangeColour;
        private GameObject[] _terrainToChangeColour;
        [Header("Add TAG in Unity!!!")]
        [Header("Add all Tags that are used on objects that should change colour!!")]
        [SerializeField] private string[] nameOfTags;
        // Boolean to check if scene is loaded
        public bool[][] _isSceneVisited;
        [Header("Build indexes of first scene the player starts in and copy of that scene")]
        [SerializeField] private int gameScene;
        [SerializeField] private int copyScene;
        private static bool _copySceneLoaded = false;
        
        private static bool _gameStarted = false;
        private IColourChange _colourChange01Implementation;
        private CrayonCounter _crayonCounter;
        //Both are pauseMenus
        private PauseMenu _pauseMenu;
        private ItemManagerSaveLogic _IMSLogic;

        private int _currentScene;
        
        // Crayons In UI
        private GameObject crayonsUI;
        private GameObject _cameraFocus;
        private Vector3 _cameraFocusPos;

        private void OnDestroy()
        {
            _copySceneLoaded = false;
            _gameStarted = false;
        }

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            
            if (!_gameStarted)
            {
                //Set all numbers of objects to 0
                //Later not make all values 0 so that the player are able to bring between world if needed
                NumbCarried = new int[txt.Length];
                NumbStored = new int[NumbCarried.Length - 1];
                
                _gameStarted = true;
            }
            //Defining the length of the 2D array
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            _objectsToChangeColour = new GameObject[NumbStored.Length][];
            _terrainToChangeColour = new GameObject[NumbStored.Length];
            _isSceneVisited = new bool[sceneCount][];
            
            for (int i = 0; i<_isSceneVisited.Length; i++)
            {
                _isSceneVisited[i] = new bool[NumbStored.Length];
                for (int j = 0; j < NumbStored.Length; j++)
                {
                    _isSceneVisited[i][j] = false;
                }
            }


            _crayonCounter = GameObject.Find("CrayonCounter").GetComponent<CrayonCounter>();
            _showColour = GameObject.Find("ShowColour").GetComponent<ShowColour>();
            _IMSLogic = GameObject.Find("MenuController").GetComponent<ItemManagerSaveLogic>();
            crayonsUI = GameObject.Find("CanvasCrayon/Crayons");
            _cameraFocus = transform.Find("CameraTarget").gameObject;
        }
        void Start()
        {
            ChangeAlienColour(0);
            UpdateValues();
            _cameraFocusPos = _cameraFocus.transform.localPosition;
        }
        //This is to change the objects that the player can colour
        //to match the scene the player "loads" into
        public void MySceneLoader()
        {
            _currentScene = SceneManager.GetActiveScene().buildIndex;
           
            // Makes so that crayons from first scene is saved to copy
            if (!_copySceneLoaded)
            {
                if (_currentScene == copyScene)
                {
                    _isSceneVisited[copyScene] = _isSceneVisited[gameScene];
                    _crayonCounter.CopyValues(gameScene, copyScene);
                    _copySceneLoaded = true;
                }
            }
            
            
            _IMSLogic.SaveValues();
            
            
            //Finds terrain in scenes to colour
            TerrainShade[] tempTerrainHolder = FindObjectsOfType<TerrainShade>();
            _terrainToChangeColour = new GameObject[tempTerrainHolder.Length];
            for(int i = 0; i < tempTerrainHolder.Length; i++) 
                _terrainToChangeColour[i] = tempTerrainHolder[i].gameObject;
            //Goes through all the colours that can be picked up
            for (int i = 0; i < nameOfTags.Length; i++)
            {
                //Find GameObjects that has EnviromentShade Script, compares GameObject colour to the ColourIndex,
                //Save those objects as a GameObject Array
                GameObject[] objectsWithEnum = FindObjectsOfType<EnviromentShade>().Where(go => go.colourToBe == (ColourHolder.Colour)i).Select(go => go.gameObject).ToArray();
                _objectsToChangeColour[i] = objectsWithEnum;
                
                //Checks bool if colour been picked up in scene previously to colour the surroundings
                if (_isSceneVisited[_currentScene][i] || NumbStored[i] > 0)
                {
                    ChangeColourOfEnvironment(i + 1);
                }
            }
            _showColour.ChangeIcon(currentColour);
            _spaceShip = GameObject.Find("SpaceShip");
            _cameraFocus.transform.localPosition = _cameraFocusPos;
            hintText.SetActive(false);
            UpdateValues();
            
            _crayonCounter.CrayonCheckup();
            
        }
        
        private void Update()
        {
            //If player enters an area with triggers "canInteract = true" they can interact with the object and based on
            //the TAG of the object different actions is executed
            if (_canInteract && Input.GetButtonDown("Interact"))
            {
                HandleInteractions();
            }
            if (Input.GetButtonDown("ChangeColour"))
            {
                ColourSwapper();
            }
            
            transform.eulerAngles = Vector3.zero;
        }
        private void OnTriggerEnter(Collider other)
        {
            _otherObject = other.gameObject;
            //When crayon is picked up
            if (_otherObject.CompareTag(nameof(Crayon)))
            {
                _otherObject.GetComponent<CrayonDisplay>().PickedUp();
                int numb = _otherObject.GetComponent<CrayonDisplay>().crayon.nr;
                currentColour = numb;
                NumbCarried[numb - 1]++;
                ChangeAlienColour(numb);
                if (_objectsToChangeColour[numb - 1].Length > 0)
                    ChangeColourOfEnvironment(numb);
                CrayonProgress++;
                
                _crayonCounter.AddCrayonToList(_otherObject);
                _isSceneVisited[_currentScene][numb-1] = true;
                UpdateValues();
                Destroy(_otherObject);
            }
            else if (_otherObject.CompareTag("SpaceShip"))
            {
                hintText.SetActive(true);
                _canInteract = true; 
            }
            
        }
        private void OnTriggerExit(Collider other)
        {
            hintText.SetActive(false);
            _canInteract = false;
        }
        public void UpdateValues()
        {
            for (int i = 0; i < txt.Length; i++)
            {
                txt[i] = crayonsUI.transform.GetChild(i).GetChild(0).gameObject;
               // print(txt[i]);
                txt[i].GetComponent<TextMeshProUGUI>().text = NumbCarried[i].ToString();
            }
            
            _showColour.ChangeIcon(currentColour);
            CrayonPickedUp?.Invoke();
        }
        // ReSharper disable Unity.PerformanceAnalysis
        void HandleInteractions()
        {
            
            if (_otherObject.transform.GetComponent(nameof(IPlayerInteract)) is IPlayerInteract)
            {
                _otherObject.GetComponent<IPlayerInteract>().PlayerInteract();
            }
            UpdateValues();
        }

        public void ChangeAlienColour(int colourIndex)
        {
            foreach (Renderer render in rend)
                render.sharedMaterial = colours[colourIndex];
        }
        // Swaps Player Colour
        private void ColourSwapper()
        {
            //Get player current colour ID in numb
            int numb = currentColour;
            int[] numbArray = new int[colours.Length];
            //Order the number so the next colour picked is next in line
            numbArray = SetOrderOfColour(numb);
            //Sends the order to check if player has colour and the colour the player has
            int[] colourObtained = new int[NumbStored.Length];
            for (int i = 0; i < NumbStored.Length; i++)
            {
                colourObtained[i] = NumbCarried[i] + NumbStored[i];
            }
            numb = SwapTo(numbArray, colourObtained);
            foreach (Renderer render in rend)
                render.sharedMaterial = colours[numb];
            currentColour = numb;
            _showColour.ChangeIcon(currentColour);
            
        }
        private int[] SetOrderOfColour(int currentNumb)
        {
            //Increase to make the loop not choose the same colour as selected but the next one
            currentNumb++;

            int[] returnArray = new int[colours.Length];

            for (int i = 0; i < colours.Length; i++)
            {
                //Sorts number so the colours always goes in the same order
                returnArray[i] = (i + currentNumb) % colours.Length;
            }
            return returnArray;
        }
        //return colourID the player swaps to
        private int SwapTo(int[] orderChecked, int[] numbOf)
        {
            int numberReturn = currentColour;
            for (int i = 0; i < orderChecked.Length; i++)
            {
                // Swap to colour if collected a crayon of it
                if (orderChecked[i] != 0 && numbOf[orderChecked[i] - 1] > 0)
                {
                    numberReturn = orderChecked[i];
                    break;
                }
            }
            return numberReturn;
        }
        public void ChangeColourOfEnvironment(int playerColourIndex)
        {
            //Check if colour is already applied
            //if (objectsToChangeColour[playerColourIndex - 1][0].transform.GetComponent<Renderer>().sharedMaterial
            //        != colours[playerColourIndex])
            {
                
                    //Find length of the row of 2D array
                foreach (var obj in _objectsToChangeColour[playerColourIndex - 1])
                {
                    //Change colour of each element in 2D array
                    if (obj.transform.GetComponent(nameof(EnviromentShade)) is EnviromentShade)
                    {
                        obj.transform.GetComponent<EnviromentShade>().SwapToShade(playerColourIndex-1);
                    }
                    else
                        obj.transform.GetComponent<Renderer>().sharedMaterial = colours[playerColourIndex];

                    if (obj.GetComponent(nameof(IColourChange)) is IColourChange)
                        obj.GetComponent<IColourChange>().ColourChange();
                    
                }
                
                
                // Checks if terrain is used in scene and sends the colour index to check if it has the colour
                foreach (var obj in _terrainToChangeColour)
                {
                    if (obj.transform.GetComponent(nameof(TerrainShade)) is TerrainShade)
                    {
                        obj.GetComponent<TerrainShade>().FindCurrentTexture(playerColourIndex-1);
                    }
                }
            }
            

            
        }

        public void MoveAlien(Vector3 spawnPos)
        {
            transform.position = spawnPos;
            transform.parent.gameObject.transform.Find("Head").transform.position = spawnPos + new Vector3(0,1,0);
            Physics.SyncTransforms();
        }
        
        //Outdated code, maybe need later though
        /* else if (otherObject.CompareTag("Other"))
            {
                numbCarried[colours.Length - 1]++;
                Destroy(otherObject.gameObject);
            }
            else if (otherObject.CompareTag("NPC"))
            {
                if (numbCarried[colours.Length - 1] > 0)
                {
                    numbCarried[colours.Length - 1]--;
                    Instantiate(crayon, new Vector3(3, 1, -7), Quaternion.identity);
                }
                else
                {
                    Debug.Log("You need items to trade");
                }
            }*/
    }
}

