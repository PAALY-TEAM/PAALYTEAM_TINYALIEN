using UnityEngine;
using UnityEngine.Serialization;

namespace Interfaces.ColourChange.Gameplay01
{
    public class RedStairs : MonoBehaviour, IColourChange
    {
        [FormerlySerializedAs("StairCollider")] [SerializeField] GameObject stairCollider;
        private MeshCollider _collider;

        private void Awake()
        {
            _collider = stairCollider.GetComponent<MeshCollider>();
            _collider.enabled = false;
        }

        public void ColourChange()
        {
            //Enable Collider
        
            _collider.enabled = true;
        }
 
    }
}

