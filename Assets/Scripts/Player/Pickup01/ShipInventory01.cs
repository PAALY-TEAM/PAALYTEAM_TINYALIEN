using Pickup01.Crayon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Pickup01
{
    public class ShipInventory01 : MonoBehaviour
    {
    
        [SerializeField] private GameObject blankCrayon;
        [Header("Scriptable Object that gives the values")]
        [SerializeField] private CrayonNumber01[] crayonColour;
        [FormerlySerializedAs("crayonOnShip")]
        [Header("How many crayons can go on the ship?")]
        [SerializeField]
        public float maxCrayonOnShip;

        private int[] visibleCrayon;

        private int p = 0;
    
    private void Start()
    {
        
        //Makes crayon and set all values to 0, Array work in the same way as any other
        visibleCrayon = new int[ItemManager01.numbStored.Length];
        System.Array.Clear(visibleCrayon, 0, visibleCrayon.Length);
    }


        public void Display()
        {
            //Number of crayons placed (p) 
        
        
            //Get radius of rotation so that it is easier when placing crayons in ship aka. automatic rather than manual
            //Using the Max number of crayon in level to calculate space
        
            float radiusToRotate = 360 / (maxCrayonOnShip);
            float radianToRotate = radiusToRotate * Mathf.Deg2Rad;

        //Repeats for each colour
            for (int i = 0; i < crayonColour.Length; i++)
            {
                int diff = ItemManager01.numbStored[i] - visibleCrayon[i];
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
                    //Change Trigger Into Collider
                    crayonMade.GetComponent<Collider>().isTrigger = false;

                    //Rotate with each placed Crayon
                    crayonMade.transform.Rotate(0, radiusToRotate * p, 15);

                    //Make so the space between changes depending on max. number of crayons with the help of Mathf
                    float x = Mathf.Cos(radianToRotate * p) * -1;
                    float y = Mathf.Sin(-radianToRotate * p) * -1;
                    crayonMade.transform.localPosition = new Vector3(x, 2.2f, y);
                    p++;
                }
                
                visibleCrayon[i] = Pickup.Player.ItemManager.numbStored[i];
            }
            
            //Checks if playerColor has stored the right amount of crayon in the ship
            if (p >= maxCrayonOnShip - 1)
                Win();
        }

        private void Win()
        {
            Cursor.visible = true;
            for (int i = 0; i < Pickup.Player.ItemManager.numbStored.Length; i++)
            {
                Pickup.Player.ItemManager.numbStored[i] = 0;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
