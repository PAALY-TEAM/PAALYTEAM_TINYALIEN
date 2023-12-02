using UnityEngine;

namespace Pickup.Shade
{
    public class ColourHolder : MonoBehaviour
    {
        public enum Element
        {
            Element0,
            Element1,
            Element2,
            Element3,
            Element4
            //If needed add more
        }
        public enum Colour

        {
            Red,
            Yellow,
            Blue,
            Green,
            Orange,
            Purple,
            Brown
        }

        public enum Shade
        {
            Normal,
            Light,
            Dark,
            Bland,
            Unique
        }

        [Header("Shades should be in this order: Normal, Light, Dark, Bland")] 
        [SerializeField] private Material[] redShades;
        [SerializeField] private Material[] yellowShades;
        [SerializeField] private Material[] blueShades;
        [SerializeField] private Material[] greenShades;
        [SerializeField] private Material[] orangeShades;
        [SerializeField] private Material[] purpleShades;
        [SerializeField] private Material[] brownShades;

        [Header("Black and White materials for each colour")]
        [SerializeField] private Material[] BWColour;

        public static Material[] BWColours;
        public static Material[][] EveryColourWithShades;
        

        private void Awake()
        {
            //Set length equal to number of colours
            EveryColourWithShades = new []
            {
                redShades,
                yellowShades,
                blueShades,
                greenShades,
                orangeShades,
                purpleShades,
                brownShades
            };
            BWColours = BWColour;
        }
    }
}
