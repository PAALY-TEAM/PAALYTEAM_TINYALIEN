using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Crayon;
using Pickup.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrayonLost : MonoBehaviour
{
    //int[Scene][Colour] = Count of lost crayon in scene
    public static int[][] crayonLost;
    //Vector3[Scene][Positions] = Position to spawn crayon
    private static Vector3[][] spawnLocations;
    // The crayon to spawn
    [SerializeField] private GameObject[] stolenCrayon;

    // Counts how many crayons stolen for placement
    private int stolenCounter;
    
    private int currentScene;

    private static int id;

    private void Start()
    {
        if (crayonLost == null)
        {
            //Declare Variable
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            print("SceneCount" + sceneCount);
            crayonLost = new int[sceneCount][];
            spawnLocations = new Vector3[sceneCount][];
            for (int i = 0; i < sceneCount; i++)
            {
                crayonLost[i] = new int[ItemManager.NumbStored.Length];
                for (int j = 0; j < crayonLost[i][j]; j++)
                {
                    // Sets all values to 0
                    crayonLost[i][j] = 0;
                }
            }
        }
      
        currentScene = SceneManager.GetActiveScene().buildIndex;
        int tempCount = 0;
        // Stops the code if empty
        if (crayonLost[currentScene]==null) return;
        for ( int i = 0; i < crayonLost[currentScene].Length; i++)
        {
            //Runs and repeats for multiple of same colour
            for (int j = 0; j < crayonLost[currentScene][i]; j++)
            {
                // Instantiate crayon at spawnLocation
                SpawnCrayon(i, tempCount);
                //Increase so that the next crayon spawns in the next location
                tempCount++;
                //Checks if the next spawn location would repeat
                if (tempCount > spawnLocations[currentScene].Length)
                {
                    tempCount = 0;
                }
            }
        }
    }

    //When player loses crayon to guards e.g.
    public void AddLostCrayon(int colourIndex, Vector3[] positions)
    {
        // Checks if the value is null and Add the Positions the crayons can spawn if so
        if (spawnLocations[currentScene] == null) spawnLocations[currentScene] = positions;

        //Spawn Crayon on position
        SpawnCrayon(colourIndex, stolenCounter);
        stolenCounter++;
        if (stolenCounter >= spawnLocations[currentScene].Length)
        {
            stolenCounter = 0;
        }
        
        // Add crayon to list by increasing the value claimed by one
        crayonLost[currentScene][colourIndex]++;
        
    }
    
    //Is called by crayon when picked up
    public void RemoveLostCrayon(int colourIndex)
    {
        //Removes one crayon of colour
        crayonLost[currentScene][colourIndex]--;
    }

    private void SpawnCrayon(int colourIndex, int numbPlace)
    {
        var crayon = Instantiate(stolenCrayon[colourIndex], spawnLocations[currentScene][numbPlace], Quaternion.identity);
        crayon.GetComponent<CrayonDisplay>().wasStolen = true;
        crayon.name = "StolenCrayon" + id;
        crayon.transform.parent = GameObject.Find("CrayonHolder").transform;
    }
}
