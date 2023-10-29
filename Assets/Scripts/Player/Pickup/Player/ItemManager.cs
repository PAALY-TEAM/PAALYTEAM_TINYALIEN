using System;
using System.Linq;
using Interfaces.ColourChange;
using Pickup.Crayon;
using Pickup.Shade;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        /// <summary>
        /// This part is to find all the objects in the scene by tag
        /// so that the different objects can change colour when the
        /// player picks up that colour
        /// The scripts continues in start
        /// </summary>
        private GameObject[][] _objectsToChangeColour;
        [Header("Add TAG in Unity!!!")]
        [Header("Add all Tags that are used on objects that should change colour!!")]
        [SerializeField] private string[] nameOfTags;

        private bool[] _isSceneVisited;

        private static bool _gameStarted = false;
        private IColourChange _colourChange01Implementation;
        

        private int _currentScene;
        private void Awake()
        {
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
            
            _isSceneVisited = new bool[sceneCount];
            for (int i = 0; i<_isSceneVisited.Length; i++)
            {
                _isSceneVisited[i] = false;
            }

        }
        void Start()
        {
            foreach (Renderer render in rend)
            {
                render.enabled = true;
                render.sharedMaterial = colours[0];
            }

            UpdateValues();
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
                foreach (Renderer render in rend) {
                    render.sharedMaterial = colours[numb];
                }
                if (_objectsToChangeColour[numb - 1].Length > 0)
                    ChangeColourOfEnvironment(numb);
                CrayonProgress++;
                UpdateValues();
                GameObject.Find("CrayonCounter").GetComponent<CrayonCounter>().ChangeThisCrayonStatus(_otherObject);
                
                Destroy(_otherObject);
                
            }
            else
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
                txt[i] = GameObject.Find("CanvasCrayon").transform.GetChild(i).GetChild(0).gameObject;
               // print(txt[i]);
                txt[i].GetComponent<TextMeshProUGUI>().text = NumbCarried[i].ToString();
            }
            //Is gameobject missing?
            if (GameObject.Find("ShowColour")!=null)
            {
                GameObject.Find("ShowColour").GetComponent<ShowColour>().ChangeIcon(currentColour);
            }
            
            CrayonPickedUp?.Invoke();
        }
        // ReSharper disable Unity.PerformanceAnalysis
        void HandleInteractions()
        {
            if (_otherObject.CompareTag("SpaceShip"))
            {
                //For future coding if we want visible display of crayons to have a origin position to the crayons
                var shipScript = _spaceShip.GetComponent<ShipInventory>();
                //Adding the crayons to ship
                //Visible in the debug log
                for (int i = 0; i < colours.Length - 1; i++)
                {
                    if (NumbCarried[i] > 0)
                    {
                        NumbStored[i] += NumbCarried[i];
                        NumbCarried[i] = 0;
                    }
                }
                shipScript.Display();
                foreach (Renderer render in rend)
                    render.sharedMaterial = colours[0];
                currentColour = 0;
            }
            //transform.GetChild(0).gameObject.SetActive(false);
            UpdateValues();
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

        }
        private int[] SetOrderOfColour(int currentNumb)
        {
            //Increase to make the loop not choose the same colour as selected but the next one
            currentNumb++;

            int[] returnArray = new int[colours.Length];

            for (int i = 0; i < colours.Length; i++)
            {

                returnArray[i] = (i + currentNumb) % (colours.Length);

                //Debug.Log(i + " " + returnArray[i]);
            }
            return returnArray;
        }
        //return colourID the player swaps to
        private int SwapTo(int[] orderChecked, int[] numbOf)
        {
            int numberReturn = currentColour;
            for (int i = 0; i < orderChecked.Length; i++)
            {
                if (orderChecked[i] != 0 && numbOf[orderChecked[i] - 1] > 0)
                {
                    numberReturn = orderChecked[i];
                    break;
                }
            }
            return numberReturn;
        }
        private void ChangeColourOfEnvironment(int numb)
        {
            //Check if colour is already applied
            //if (objectsToChangeColour[numb - 1][0].transform.GetComponent<Renderer>().sharedMaterial != colours[numb])
            {
                //Find length of the row of 2D array
                foreach (var obj in _objectsToChangeColour[numb - 1])
                {
                    //Change colour of each element in 2D array
                    if (obj.transform.GetComponent(nameof(EnviromentShade)) is EnviromentShade)
                    {
                        obj.transform.GetComponent<EnviromentShade>().SwapToShade(numb-1);
                    }
                    else
                        obj.transform.GetComponent<Renderer>().sharedMaterial = colours[numb];

                    if (obj.GetComponent(nameof(IColourChange)) is IColourChange)
                    {
                        obj.GetComponent<IColourChange>().ColourChange();
                    } 
                }
            }
        }

        //This is to change the objects that the player can colour
        //to match the scene the player "loads" into
        public void MySceneLoader()
        {
            _currentScene = SceneManager.GetActiveScene().buildIndex;
            
                for (int i = 0; i < nameOfTags.Length; i++)
                {
                    //Find GameObjects that has EnviromentShade Script, compares GameObject colour to the ColourIndex, Save those objects as a GameObject Array
                    GameObject[] objectsWithEnum = FindObjectsOfType<EnviromentShade>().Where(go => go.colourToBe == (ColourHolder.Colour)i).Select(go => go.gameObject).ToArray();
                    _objectsToChangeColour[i] = objectsWithEnum;
                    if (NumbStored[i] > 0 || NumbCarried[i] > 0)
                    {
                        if (_isSceneVisited[_currentScene])
                        {
                            ChangeColourOfEnvironment(i + 1);
                        }
                    }
                    

                }

            
            _isSceneVisited[_currentScene] = true;
            _spaceShip = GameObject.Find("SpaceShip");
            hintText = GameObject.Find("Hint");
            hintText.SetActive(false);
            UpdateValues();
            GameObject.Find("CrayonCounter").GetComponent<CrayonCounter>().CrayonCheckup();
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
