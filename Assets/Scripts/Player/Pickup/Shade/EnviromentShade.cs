using System;
using UnityEngine;

namespace Pickup.Shade
{
    public class EnviromentShade : MonoBehaviour
    {
        [Header("Element/part to colour of object")]
        [SerializeField] private ColourHolder.Element[] elementToColour;
        [Header("Colour that it should turn into")]
        public ColourHolder.Colour[] colourToBe;
        [Header("The shade/version of the colour")]
        [SerializeField] private ColourHolder.Shade[] shadeType;
        [Header("If the object needs an unique material put that here")]
        [SerializeField] private Material[] uniqueMaterial;

        private Material[] changeTo;
        private Renderer rend;

        public void ColourStart()
        {
            rend = gameObject.GetComponent<MeshRenderer>();
            changeTo = new Material[elementToColour.Length];
            if (rend == null)
            {
                Debug.LogError("Renderer component not found on the GameObject.");
            }

            if (elementToColour.Length != colourToBe.Length || colourToBe.Length != shadeType.Length)
            {
                Debug.LogError(gameObject.name+ " does not have arrays with equal length");
            }

            for (int i = 0; i < elementToColour.Length; i++)
            {
                // Change Material to gray of the same pattern as the coloured
                if (shadeType[i] is ColourHolder.Shade.Bland or ColourHolder.Shade.Unique)
                {
                    // If bland or unique shade go gray with no pattern
                    ChangeMaterial(ColourHolder.BWColours[^1], i);
                }
                else
                {
                    ChangeMaterial(ColourHolder.BWColours[(int)colourToBe[i]], i);
                }

                if (shadeType[i] != ColourHolder.Shade.Unique)
                {
                    // Saves the material to change to by selecting the colour and the shade.
                    changeTo[i] = ColourHolder.EveryColourWithShades[(int)colourToBe[i]][(int)shadeType[i]];
                }
            }
            
            
        }

        public void SwapToShade(int index)
        {
            for (int i = 0; i < elementToColour.Length; i++)
            {
                // Check that colour picked up matches the colour to change to
                if (index == (int)colourToBe[i])
                {
                    // If using unique material
                    if (shadeType[i] == ColourHolder.Shade.Unique)
                    {
                        ChangeMaterial(uniqueMaterial[i], i);
                        return;
                    }
                    // Change Material to the colour selected
                    ChangeMaterial(changeTo[i], i);
                }
            }
            
        }
        private void ChangeMaterial(Material mat,int i)
        {
            var tempMaterialsHolder = rend.materials;
            tempMaterialsHolder[(int)elementToColour[i]] = mat;
            rend.materials = tempMaterialsHolder;
        }
    }
}
