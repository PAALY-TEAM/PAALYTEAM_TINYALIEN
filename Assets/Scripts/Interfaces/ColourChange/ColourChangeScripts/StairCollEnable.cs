using Pickup.Shade;
using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class StairCollEnable : MonoBehaviour, IColourChange
    {
        private MeshCollider _collider;
        private MeshCollider _childCollider;

        private void Awake()
        {
            _collider = GetComponent<MeshCollider>();
            _childCollider = transform.GetChild(0).GetComponent<MeshCollider>();
            _collider.enabled = false;
            _childCollider.enabled = false;
        }

        public void ColourChange(int colourIndex)
        {
            if (colourIndex == (int)GetComponent<EnviromentShade>().colourToBe[0])
            {
                //Enable Collider
                _collider.enabled = true;
                _childCollider.enabled = true;
            }
        }
    }
}
