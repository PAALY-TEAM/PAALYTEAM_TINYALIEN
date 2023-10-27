using UnityEngine;

namespace Pickup.Crayon
{
    [CreateAssetMenu(fileName = "New Crayon", menuName = "Crayon")]
    public class CrayonNumber : ScriptableObject
    {
        [Tooltip("Number for the colourchange of Player")]
        public int nr;

        [Tooltip("The colour of the crayon  ")]
        public Material[] colour;

    
    }
}
