using UnityEngine;

namespace Pickup.Shade
{
    public class EnviromentShade : MonoBehaviour
    {
    
        public ColourHolder.Colour colourToBe;
        [SerializeField] private ColourHolder.Shade shadeType;

        private Material[] _shadeList;

        public void SwapToShade(int colourIndex)
        {
            var shadeHolder = GameObject.Find("ColourHolder").GetComponent<ColourHolder>();
            _shadeList = shadeHolder.EveryColourWithShades[(int)colourToBe];
            GetComponent<Renderer>().sharedMaterial = _shadeList[(int)shadeType];
        }
    }
}
