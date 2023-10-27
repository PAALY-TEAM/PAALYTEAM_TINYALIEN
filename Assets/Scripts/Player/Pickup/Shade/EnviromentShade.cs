using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnviromentShade : MonoBehaviour
{
    
    [SerializeField] private ColourHolder.Colour colourToBe;
    [SerializeField] private ColourHolder.Shade shadeType;

    private Material[] shadeList;

    public void SwapToShade(int colourIndex)
    {
        var shadeHolder = GameObject.Find("ColourHolder").GetComponent<ColourHolder>();
        shadeList = shadeHolder.EveryColourWithShades[colourIndex];
        GetComponent<Renderer>().sharedMaterial = shadeList[(int)shadeType];
    }
}
