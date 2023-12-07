using MoreMountains.Feedbacks;
using System;
using UnityEngine;

namespace Pickup.Crayon
{
    public class CrayonDisplay : MonoBehaviour
    {
        public CrayonNumber crayon;
        public bool isSpinning;

        private Renderer _rend;

        public bool wasStolen;

        private CrayonLost _crayonLost;

        [SerializeField] private MMFeedbacks pickupFeedback;

        private void Start()
        {
            _rend = GetComponent<Renderer>();
            _rend.enabled = true;

            if (isSpinning)
                _rend.sharedMaterial = crayon.colour[0];

            if (GameObject.Find("CrayonLost"))
                _crayonLost = GameObject.Find("CrayonLost").GetComponent<CrayonLost>();
        }

        void Update()
        {
            if (isSpinning)
                transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

        public void PickedUp()
        {
            pickupFeedback?.PlayFeedbacks();

            if (!GameObject.Find("CrayonLost")) return;
            if (wasStolen)
            {
                print("I was stolen D:");
                _crayonLost.RemoveLostCrayon(crayon.nr -1);
            }
        }
    }
}