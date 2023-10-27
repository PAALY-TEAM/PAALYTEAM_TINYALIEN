using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pickup01
{
    public class ItemManager01 : MonoBehaviour
    {
        [Header("Add gameObject to change the text of")]
        [SerializeField] private GameObject[] txt;
        //Number of collected objects
        public static int[] numbCarried;
        //Number of stored objects in SpaceShip
        public static int[] numbStored;
        //Player not carrying crayons
        private bool empty = true;
        [Header("Crayon to instantiate for NPC")]
        [SerializeField] private GameObject crayon;
        [Header("Number of colours and wich that the playerColor can turn into")]
        [SerializeField] public Material[] colours;
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
        //Objects that playerColor needs information from (pos, scripts etc.)
        private GameObject spaceShip;
        //To save the trigger (other) GameObject
        private GameObject otherObject;
        //The playerColor is in range to interact with NPC
        private bool canInteract = false;
        //Text to make it easier for playerColor to understand something
        private GameObject hintText;
        /// <summary>
        /// This part is to find all the objects in the scene by tag
        /// so that the different objects can change colour when the
        /// playerColor picks up that colour
        /// The scripts continues in start
        /// </summary>
        private GameObject[][] objectsToChangeColour;
        [Header("Add TAG in Unity!!!")]
        [Header("Add all Tags that are used on objects that should change colour!!")]
        [SerializeField] private string[] nameOfTags;

        private static bool gameStarted = false;
        private IColourChange01 _colourChange01Implementation;

        private void Awake()
        {
            if (!gameStarted)
            {
                //Set all numbers of objects to 0
                //Later not make all values 0 so that the playerColor are able to bring between world if needed
                numbCarried = new int[txt.Length];
                numbStored = new int[numbCarried.Length - 1];
                gameStarted = true;
            }
            //Defining the length of the 2D array
            objectsToChangeColour = new GameObject[numbStored.Length][];
            spaceShip = GameObject.Find("SpaceShip");
            hintText = GameObject.Find("Hint");
        }
        void Start()
        {
            hintText.SetActive(false);
            foreach (Renderer render in rend)
            {
                render.enabled = true;
                render.sharedMaterial = colours[0];
            }
            for (int i = 0; i < numbStored.Length; i++)
            {
                GameObject[] objectWithTheTag = GameObject.FindGameObjectsWithTag(nameOfTags[i]);
                objectsToChangeColour[i] = objectWithTheTag;

                if (numbStored[i] > 0 || numbCarried[i] > 0)
                {
                    ChangeColourOfEnvironment(i+1);
                }
            }

            UpdateValues();
        }
        private void Update()
        {
            //If playerColor enters an area with triggers "canInteract = true" they can interact with the object and based on
            //the TAG of the object different actions is executed
            if (canInteract && Input.GetButtonDown("Interact"))
            {
                HandleInteractions();
            }
            if (Input.GetButtonDown("ChangeColour"))
            {
                if (spaceShip.activeInHierarchy == true)
                {
                    float shipDistance = Vector3.Distance(transform.position, spaceShip.transform.position);
                    if (empty && shipDistance < 1)
                        ColourSwapper(numbStored, true);
                    else
                        ColourSwapper(numbCarried, false);
                }
                else
                    ColourSwapper(numbCarried, false);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Crayon"))
            {
                CrayonPickedUp?.Invoke();
                CrayonDisplay01 crayonDisplay = other.GetComponent<CrayonDisplay01>();
                crayonDisplay.PickedUp();
                int crayonNumber = crayonDisplay.crayon.nr;
                currentColour = crayonNumber;
                numbCarried[crayonNumber - 1]++;
                foreach (Renderer render in rend)
                {
                    render.sharedMaterial = colours[crayonNumber];
                }
                if (objectsToChangeColour[crayonNumber - 1].Length > 0)
                {
                    ChangeColourOfEnvironment(crayonNumber);
                }
                CrayonProgress++;
                CrayonPickedUp?.Invoke();
                Destroy(other.gameObject);
            }
            else
            {
                otherObject = other.gameObject;
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
                txt[i].GetComponent<TextMeshProUGUI>().text = numbCarried[i].ToString();
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        void HandleInteractions()
        {
            if (otherObject.CompareTag("SpaceShip"))
            {
                var shipScript = spaceShip.GetComponent<ShipInventory01>();
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
                empty = true;
            }
            hintText.SetActive(false);
            canInteract = false;
            UpdateValues();
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void ColourSwapper(int[] colourObtained, bool isShip)
        {
            //Change players current numbering of Colour
            int numb = currentColour;
            int[] numbArray = new int[colours.Length];
            //Order the number so the next colour picked is next in line
            numbArray = SetOrderOfColour(numb, isShip);
            //Sends the order to check if playerColor has colour and the colour the playerColor has
            numb = SwapTo(numbArray, colourObtained);
            foreach (Renderer render in rend)
                render.sharedMaterial = colours[numb];
            currentColour = numb;

        }
        private int[] SetOrderOfColour(int currentNumb, bool isShip)
        {
            //Increase to make the loop not choose the same colour as selected but the next one
            currentNumb++;

            int[] returnArray = new int[colours.Length];

            for (int i = 0; i < colours.Length; i++)
            {

                returnArray[i] = (i + currentNumb) % (colours.Length);

                Debug.Log(i + " " + returnArray[i]);
            }
            return returnArray;
        }
        //FIX THIS; THE PROBLEM IS THAT THE ARRAYS THAT ARE INSERTED FROM NUBOF HAVE DIFFERENT SIZES
        private int SwapTo(int[] orderChecked, int[] numbOf)
        {
            int numberReturn = 0;
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
            
                //Find length of the row of 2D array
                foreach (var obj in objectsToChangeColour[numb - 1])
                {
                    //Change colour of each element in 2D array
                    obj.transform.GetComponent<Renderer>().sharedMaterial = colours[numb];

                    IColourChange01 sd = obj.GetComponent("IColourChange01") as IColourChange01;
                    if (sd != null)
                    {
                        obj.GetComponent<IColourChange01>().ColourChange01();
                    } 
                }
        }

        //This is to change the objects that the playerColor can colour
        //to match the scene the playerColor "loads" into
        public void MySceneLoader()
        {
            objectsToChangeColour = new GameObject[numbStored.Length][];
            
            for (int i = 0; i < numbStored.Length; i++)
            {
                GameObject[] objectWithTheTag = GameObject.FindGameObjectsWithTag(nameOfTags[i]);
                objectsToChangeColour[i] = objectWithTheTag;
            }
        }
    }
}

