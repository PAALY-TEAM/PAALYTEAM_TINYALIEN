using UnityEngine;

namespace Pickup01
{
    public class Sun : MonoBehaviour, IColourChange
    {
        private GameObject dirLight;
        private Light myLight;

        void Awake()
        {
            dirLight = GameObject.Find("Directional Light");
            myLight = dirLight.GetComponent<Light>();
        }

        public void ColourChange()
        {
            myLight.intensity = 1;
        }
    }
}
