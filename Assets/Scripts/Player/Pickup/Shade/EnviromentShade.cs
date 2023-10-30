using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnviromentShade : MonoBehaviour
{
    
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
    }
}
