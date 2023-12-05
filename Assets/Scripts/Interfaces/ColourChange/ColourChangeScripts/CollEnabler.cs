using Pickup.Shade;
using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class CollEnabler : MonoBehaviour, IColourChange
    {
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        public void ColourChange(int colourIndex)
        {
            if (colourIndex == (int)GetComponent<EnviromentShade>().colourToBe[0])
                //Enable Collider
                _collider.enabled = true;
        }

    }
}
