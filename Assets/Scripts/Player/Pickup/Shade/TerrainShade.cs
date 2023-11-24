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

        public TerrainChange(ColourHolder.Colour ctc, int @if, int it)
        {
            ColourThatChanges = ctc;
            IndexFrom = @if;
            IndexTo = it;
        }
    }
    private Terrain _thisTarrain;

    [Header("GroundTexture Indexes")]
    // The values needed to be added for good game
    [SerializeField] private ColourHolder.Colour[] affectingColourGround;
    [SerializeField] private int[] startIndex;
    [SerializeField] private int[] endIndex;
    
    private TerrainChange[] _texturesThatChanges;
    
    // The layers that should appear
    [Header("GrassTexture Indexes")]
    [SerializeField] private ColourHolder.Colour[] affectingColourGrass;
    //[SerializeField] private int[] indexesToGrassLayers;
    private int [][,] _storedGrassDetails;
    
    private void Awake()
    {
        _thisTarrain = GetComponent<Terrain>();
        
        _texturesThatChanges = new TerrainChange[affectingColourGround.Length];
        // Initialized all values into the constructor
        for (int i = 0; i < _texturesThatChanges.Length; i++)
        {
            _texturesThatChanges[i] = new TerrainChange(affectingColourGround[i], startIndex[i], endIndex[i]);
        }

        _storedGrassDetails = new int[affectingColourGrass.Length][,];

        for (int i = 0; i < affectingColourGrass.Length; i++)
        {
            // Store the OG grass
            int detailResolution = _thisTarrain.terrainData.detailResolution;
            _storedGrassDetails[i] = _thisTarrain.terrainData.GetDetailLayer(0, 0, detailResolution, detailResolution,  i);
            
            SetGrassEnabled(true, i, _storedGrassDetails[i]);
        }
        
    }
    
    //So that the textures changes back after scene is finished
    private void OnDestroy()
    {
        foreach (var ttc in _texturesThatChanges)
        {
            UpdateTerrainTexture(ttc.IndexTo, ttc.IndexFrom);
        }
        for (int i = 0; i < affectingColourGrass.Length; i++)
        {
            SetGrassEnabled(false, i, _storedGrassDetails[i]);
        }
    }
    
    //Called by other scripts to find which textures to swap in this script
    public void FindCurrentTexture(int colourIndex)
    {
        foreach (var ttc in _texturesThatChanges)
        {
            if ((int)ttc.ColourThatChanges == colourIndex)
            {
                UpdateTerrainTexture(ttc.IndexFrom,ttc.IndexTo);   
            }
        }

        for (int i = 0; i < affectingColourGrass.Length; i++)
        {
            if ((int)affectingColourGrass[i] == colourIndex)
            {
                SetGrassEnabled(false, i, _storedGrassDetails[i]);
            }
        }
    }
    //Swaps the texture of terrain so it becomes coloured
    private void UpdateTerrainTexture(int textureNumberFrom, int textureNumberTo)
    {
        var thisTerrainData = _thisTarrain.terrainData;
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
    
    private void SetGrassEnabled(bool disable, int grassLayerIndex, int[,] savedGrass)
    {
        // Get the detail resolution of the specified detail layer
        int detailResolution = _thisTarrain.terrainData.detailResolution;
        int[,] details = _thisTarrain.terrainData.GetDetailLayer(0, 0, detailResolution, detailResolution, grassLayerIndex);

        // Set the grass density to 0 or 1 based on the enabled parameter
        for (int x = 0; x < detailResolution; x++)
        {
            for (int y = 0; y < detailResolution; y++)
            {
                if (disable)
                {
                    details[x, y] = 0;
                }
                else
                {
                    details[x, y] = savedGrass[x, y];
                }
                
            }
        }

        // Apply the modified detail layer back to the terrain
        _thisTarrain.terrainData.SetDetailLayer(0, 0, grassLayerIndex, details);
    }

    
}

