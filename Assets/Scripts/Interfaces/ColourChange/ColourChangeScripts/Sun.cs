using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class Sun : MonoBehaviour, IColourChange
    {
        private GameObject _dirLight;
        private Light _myLight;

        void Awake()
        {
            _dirLight = GameObject.Find("Directional Light");
            _myLight = _dirLight.GetComponent<Light>();
        }

        public void ColourChange()
        {
            _myLight.intensity = 1;
        }
    }
}
