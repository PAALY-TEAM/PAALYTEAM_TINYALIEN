using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pickup
{
    public class CrayonCounter : MonoBehaviour
    {
        //<SceneIndex><Int> = nameOfCrayonInScene
        public List<List<string>> savedCrayon;
        
        //Temp Crayons in scene
        private GameObject[] temp;

        private void Awake()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            savedCrayon = new List<List<string>>();
            print("SceneCount: "+ sceneCount);
            for (int i = 0; i < sceneCount; i++)
            {
                savedCrayon.Add(new List<string>());
            }
        }
        //Checks if crayon has been picked up by comparing the names in list of savedCrayon
        public void CrayonCheckup()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            // Stores all crayons in scene
            temp = GameObject.FindGameObjectsWithTag("Crayon");
         
            // Compares savedCrayons with crayons in scene
            for (int i = 0; i < savedCrayon[currentScene].Count; i++)
            {
                foreach (var t in temp)
                {
                    // If names match this loop break and goes on to check the next crayon in scene
                    if (savedCrayon[currentScene][i] == t.name)
                    {
                        t.SetActive(false);
                        break;
                    }
                }
            }
        }
        
        public void AddCrayonToList(GameObject thisCrayon)
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            savedCrayon[currentScene].Add(thisCrayon.name);
        }

        public void CopyValues(int from, int to)
        {
            savedCrayon[to] = savedCrayon[from];
        }
    }
}


