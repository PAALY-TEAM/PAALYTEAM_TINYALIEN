using System;
using UnityEngine;

namespace Pickup.Crayon
{
    public class CrayonDisplay : MonoBehaviour
    {
        public CrayonNumber crayon;
        public bool isSpinning;

        private Renderer _rend;

        private bool _isPickedUp;

        private void Awake()
        {
            if (_isPickedUp)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            
            _rend = GetComponent<Renderer>();
            _rend.enabled = true;

            if (isSpinning)
                _rend.sharedMaterial = crayon.colour[0];
        }
        void Update()
        {
            if (isSpinning)
                transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

        public void PickedUp()
        {
            _isPickedUp = true;
        }
    }
}
