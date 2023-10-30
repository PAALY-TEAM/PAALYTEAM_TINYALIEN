using UnityEngine;

namespace Pickup.Shade
{
<<<<<<< HEAD
    
    public ColourHolder.Colour colourToBe;
    [SerializeField] private ColourHolder.Shade shadeType;
    [SerializeField] private Material uniqueMaterial;

    private Material[] shadeList;

    public void SwapToShade(int colourIndex)
    {
        if (shadeType == ColourHolder.Shade.Unique)
        {
            GetComponent<Renderer>().sharedMaterial = uniqueMaterial;
            return;
        }
        var shadeHolder = GameObject.Find("ColourHolder").GetComponent<ColourHolder>();
        shadeList = shadeHolder.EveryColourWithShades[(int)colourToBe];
        GetComponent<Renderer>().sharedMaterial = shadeList[(int)shadeType];
=======
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
>>>>>>> 6fda68824c1579a99dbb80970c19fa3731b86fca
    }
}
