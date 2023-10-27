using UnityEngine;

namespace Pickup01
{
    public class RedStairs : MonoBehaviour, IColourChange
    {
        private Collider coll;

        private void Awake()
        {
            coll = gameObject.GetComponent<Collider>();
        }

        public void ColourChange()
        {
            //Enable Collider
        
            coll.enabled = true;
        }
 
    }
}

