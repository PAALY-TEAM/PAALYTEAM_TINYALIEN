using UnityEngine;

namespace Pickup01
{
    public class Hideable01 : MonoBehaviour, IColourChange
    {
        [Header("Needs this number to be the same as the layer that obscures the vision of the guards")]
        [SerializeField] private int layerNr = 7;
        public void ColourChange()
        {
            gameObject.layer = layerNr;
        }
 
    }
}

