using Pickup.Shade;
using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class Hideable : MonoBehaviour, IColourChange
    {
        [Header("Needs this number to be the same as the layer that obscures the vision of the guards")]
        [SerializeField] private int layerNr = 7;
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        public void ColourChange(int colourIndex)
        {
            if (colourIndex == (int)GetComponent<EnviromentShade>().colourToBe[0])
            {
                gameObject.layer = layerNr;
                _collider.enabled = true;
            }
        }

    }
}

