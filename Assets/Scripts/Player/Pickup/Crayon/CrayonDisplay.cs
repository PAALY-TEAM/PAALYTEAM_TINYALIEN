using System;
using UnityEngine;

namespace Pickup.Crayon
{
    public class CrayonDisplay : MonoBehaviour
    {
        public CrayonNumber crayon;
        public bool isSpinning;

        private Renderer rend;

        private bool isPickedUp;

        private void Awake()
        {
            if (isPickedUp)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            
            rend = GetComponent<Renderer>();
            rend.enabled = true;

            if (isSpinning)
                rend.sharedMaterial = crayon.colour[0];
        }
        void Update()
        {
            if (isSpinning)
                transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

        public void PickedUp()
        {
            isPickedUp = true;
        }
    }
}
