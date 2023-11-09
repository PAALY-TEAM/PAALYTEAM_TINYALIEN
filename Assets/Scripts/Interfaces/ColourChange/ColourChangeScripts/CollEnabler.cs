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

        public void ColourChange()
        {
            //Enable Collider
        
            _collider.enabled = true;
        }

    }
}
