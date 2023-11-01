using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Shade;
using UnityEngine;
using UnityEngine.Serialization;

public class TerrainShade : MonoBehaviour
{
    // Class to hold all values needed to change colour of texture:
    // The colour of the crayon needed to change the colour of terrain
    // The terrains texture index as it is in play
    // The texture index to change to
    class TerrainChange
    {
        public ColourHolder.Colour ColourThatChanges;
        public int IndexFrom;
        public int IndexTo;

        public TerrainChange(ColourHolder.Colour CTC, int IF, int IT)
        {
            ColourThatChanges = CTC;
            IndexFrom = IF;
            IndexTo = IT;
        }
    }
    private Terrain thisTarrain;

    // The values needed to be added for good game
    [SerializeField] private ColourHolder.Colour[] affectingColour;
    [SerializeField] private int[] startIndex;
    [SerializeField] private int[] endIndex;
    
    private TerrainChange[] _texturesThatChanges;
    
    
    private void Start()
    {
        thisTarrain = GetComponent<Terrain>();
        _texturesThatChanges = new TerrainChange[affectingColour.Length];
        // Initialized all values into the constructor
        for (int i = 0; i < _texturesThatChanges.Length; i++)
        {
            _texturesThatChanges[i] = new TerrainChange(affectingColour[i], startIndex[i], endIndex[i]);
        }
    }
    
    //So that the textures changes back after scene is finished
    private void OnDestroy()
    {
        foreach (var ttc in _texturesThatChanges)
        {
            UpdateTerrainTexture(ttc.IndexTo, ttc.IndexFrom);
        }
    }
    
    //Called by other scripts to find which textures to swap
    public void FindCurrentTexture(int colourIndex)
    {
        foreach (var ttc in _texturesThatChanges)
        {
            if ((int)ttc.ColourThatChanges == colourIndex)
            {
                UpdateTerrainTexture(ttc.IndexFrom,ttc.IndexTo);   
            }
        }
    }
    //Swaps the texture of terrain so it becomes coloured
    private void UpdateTerrainTexture(int textureNumberFrom, int textureNumberTo)
    {
        var thisTerrainData = thisTarrain.terrainData;
        float[, ,] alphas = thisTerrainData.GetAlphamaps(0, 0, thisTerrainData.alphamapWidth, thisTerrainData.alphamapHeight);
        
        
        for (int i = 0; i < thisTerrainData.alphamapWidth; i++)
        {
            for (int j = 0; j < thisTerrainData.alphamapHeight; j++)
            {
                //for each point of mask do:
                //paint all from old texture to new texture (saving already painted in new texture)
                alphas[i, j, textureNumberTo] = Mathf.Max(alphas[i, j, textureNumberFrom], alphas[i, j, textureNumberTo]);
                //set old texture mask to zero
                alphas[i, j, textureNumberFrom] = 0f;
            }
        }
        // apply the new alpha
        thisTerrainData.SetAlphamaps(0, 0, alphas);
    }
}

