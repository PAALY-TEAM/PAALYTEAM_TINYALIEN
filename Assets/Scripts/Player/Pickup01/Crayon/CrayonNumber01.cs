using UnityEngine;

namespace Pickup01.Crayon
{
    [CreateAssetMenu(fileName = "01_Crayon", menuName = "Crayon01")]
    public class CrayonNumber01 : ScriptableObject
    {
        [Tooltip("Number for the colour-change of Player")]
        public int nr;

        [Tooltip("The colour of the crayon  ")]
        public Material[] colour;

    
    }
}
