using Pickup01.Crayon;
using UnityEngine;

namespace Pickup01
{
    public class CrayonDisplay01 : MonoBehaviour
    {
        public CrayonNumber01 crayon;
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
