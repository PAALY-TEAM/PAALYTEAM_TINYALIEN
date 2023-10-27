using Pickup.Crayon;
using Pickup.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Pickup
{
    public class ShipInventory : MonoBehaviour
    {
    
        [SerializeField] private GameObject blankCrayon;
        [Header("Scriptable Object that gives the values")]
        public CrayonNumber[] crayonColour;
        [Header("How many crayons can go on the ship?")]
        public float maxCrayonOnShip;

        private int[] visibleCrayon;

        [HideInInspector] public int p = 0;
    
    private void Start()
    {
        
        //Makes crayon and set all values to 0, Array work in the same way as any other
        visibleCrayon = new int[ItemManager.numbStored.Length];
        System.Array.Clear(visibleCrayon, 0, visibleCrayon.Length);
        
        Display();
    }


        public void Display()
        {
            //Number of crayons placed (p) 
            print("Number of Crayons in ship before" + p );
        
            //Get radius of rotation so that it is easier when placing crayons in ship aka. automatic rather than manual
            //Using the Max number of crayon in level to calculate space
        
            float radiusToRotate = 360 / (maxCrayonOnShip);
            float radianToRotate = radiusToRotate * Mathf.Deg2Rad;

        //Repeats for each colour
            for (int i = 0; i < crayonColour.Length; i++)
            {
                int diff = ItemManager.numbStored[i] - visibleCrayon[i];
                //Repeats for each in one colour
                for (int j = 0; j < diff; j++)
                {
                    //Place Crayon visible on ship
                    GameObject crayonMade = Instantiate(blankCrayon, transform.position, Quaternion.identity);
                    //Make Crayon Child of Ship
                    crayonMade.transform.parent = transform;
                    //Put on Colour
                    Renderer rend = crayonMade.GetComponent<Renderer>();
                    rend.enabled = true;
                    rend.sharedMaterial = crayonColour[i].colour[0];

                    //Rotate with each placed Crayon
                    crayonMade.transform.Rotate(0, radiusToRotate * p, 15);

                    //Make so the space between changes depending on max. number of crayons with the help of Mathf
                    float x = Mathf.Cos(radianToRotate * p) * -1;
                    float y = Mathf.Sin(-radianToRotate * p) * -1;
                    crayonMade.transform.localPosition = new Vector3(x, 2.5f, y);
                    p++;
                }
                
                visibleCrayon[i] = ItemManager.numbStored[i];
            }

            print("After: " + p);
            GetComponent<WhenCollectedCrayons>().CheckIfEnough();
            //Checks if playerColor has stored the right amount of crayon in the ship
            if (p >= maxCrayonOnShip)
                Win();
        }

        private void Win()
        {
            Cursor.visible = true;
            for (int i = 0; i < ItemManager.numbStored.Length; i++)
            {
                ItemManager.numbStored[i] = 0;
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameCompletion");
        }
    }
}
