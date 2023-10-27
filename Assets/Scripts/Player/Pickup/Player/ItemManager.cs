using System;
using System.Linq;
using Pickup.Crayon;
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
        public static int[] numbCarried;
        //Number of stored objects in SpaceShip
        public static int[] numbStored;
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
        private GameObject spaceShip;
        //To save the trigger (other) GameObject
        private GameObject otherObject;
        //The player is in range to interact with NPC
        private bool canInteract = false;
        //Text to make it easier for player to understand something
        public GameObject hintText;
        /// <summary>
        /// This part is to find all the objects in the scene by tag
        /// so that the different objects can change colour when the
        /// player picks up that colour
        /// The scripts continues in start
        /// </summary>
        private GameObject[][] objectsToChangeColour;
        [Header("Add TAG in Unity!!!")]
        [Header("Add all Tags that are used on objects that should change colour!!")]
        [SerializeField] private string[] nameOfTags;

        private bool[] isSceneVisited;

        private static bool gameStarted = false;
        private IColourChange _colourChange01Implementation;
        

        private int currentScene;
        private void Awake()
        {
            if (!gameStarted)
            {
                //Set all numbers of objects to 0
                //Later not make all values 0 so that the player are able to bring between world if needed
                numbCarried = new int[txt.Length];
                numbStored = new int[numbCarried.Length - 1];
                
                gameStarted = true;
            }
            //Defining the length of the 2D array
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            objectsToChangeColour = new GameObject[numbStored.Length][];
            
            isSceneVisited = new bool[sceneCount];
            for (int i = 0; i<isSceneVisited.Length; i++)
            {
                isSceneVisited[i] = false;
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
            if (canInteract && Input.GetButtonDown("Interact"))
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
            otherObject = other.gameObject;
            //When crayon is picked up
            if (otherObject.CompareTag(nameof(Crayon)))
            {
                otherObject.GetComponent<CrayonDisplay>().PickedUp();
                int numb = otherObject.GetComponent<CrayonDisplay>().crayon.nr;
                currentColour = numb;
                numbCarried[numb - 1]++;
                foreach (Renderer render in rend) {
                    render.sharedMaterial = colours[numb];
                }
                if (objectsToChangeColour[numb - 1].Length > 0)
                    ChangeColourOfEnvironment(numb);
                CrayonProgress++;
                UpdateValues();
                GameObject.Find("CrayonCounter").GetComponent<CrayonCounter>().ChangeThisCrayonStatus(otherObject);
                
                Destroy(otherObject);
                
            }
            else
            {
                hintText.SetActive(true);
                canInteract = true;
            }
            
        }
        private void OnTriggerExit(Collider other)
        {
            hintText.SetActive(false);
            canInteract = false;
        }
        public void UpdateValues()
        {
            for (int i = 0; i < txt.Length; i++)
            {
                txt[i] = GameObject.Find("CanvasCrayon").transform.GetChild(i).GetChild(0).gameObject;
               // print(txt[i]);
                txt[i].GetComponent<TextMeshProUGUI>().text = numbCarried[i].ToString();
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
            if (otherObject.CompareTag("SpaceShip"))
            {
                //For future coding if we want visible display of crayons to have a origin position to the crayons
                var shipScript = spaceShip.GetComponent<ShipInventory>();
                //Adding the crayons to ship
                //Visible in the debug log
                for (int i = 0; i < colours.Length - 1; i++)
                {
                    if (numbCarried[i] > 0)
                    {
                        numbStored[i] += numbCarried[i];
                        numbCarried[i] = 0;
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
            int[] colourObtained = new int[numbStored.Length];
            for (int i = 0; i < numbStored.Length; i++)
            {
                colourObtained[i] = numbCarried[i] + numbStored[i];
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
                foreach (var obj in objectsToChangeColour[numb - 1])
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
            currentScene = SceneManager.GetActiveScene().buildIndex;
            
                for (int i = 0; i < nameOfTags.Length; i++)
                {
                    //Find GameObjects that has EnviromentShade Script, compares GameObject colour to the ColourIndex, Save those objects as a GameObject Array
                    GameObject[] objectsWithEnum = FindObjectsOfType<EnviromentShade>().Where(go => go.colourToBe == (ColourHolder.Colour)i).Select(go => go.gameObject).ToArray();
                    objectsToChangeColour[i] = objectsWithEnum;
                    if (numbStored[i] > 0 || numbCarried[i] > 0)
                    {
                        if (isSceneVisited[currentScene])
                        {
                            ChangeColourOfEnvironment(i + 1);
                        }
                    }
                    

                }

            
            isSceneVisited[currentScene] = true;
            spaceShip = GameObject.Find("SpaceShip");
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

