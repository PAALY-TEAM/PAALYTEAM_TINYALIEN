using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pickup
{
    public class CrayonCounter : MonoBehaviour
    {
        /*
        private bool _isFirstTime = true;
    
    
        private Scene[] _scenesInBuild;

        //[Current Scene Index][Index to crayon in scene]
        private int[][] _crayonHolder;

        GameObject[] _tempHolder;
        private int _currentScene;
    
        List<CrayonValues> _crayList = new List<CrayonValues>();
        private int _id = 0;

        private void Awake()
        {
            if (_isFirstTime)
            {
                var sceneCount = SceneManager.sceneCountInBuildSettings;
                _scenesInBuild = new Scene[sceneCount];
                _crayonHolder = new int[sceneCount][];

                //Go through specific scenes for when we have multiple levels/worlds
            
                for (int i = 0; i < sceneCount; i++)
                {
                    _scenesInBuild[i] = SceneManager.GetSceneByBuildIndex(i);
                }
                _crayList.Clear();

                _isFirstTime = false;
            }
        
        }

        public void CrayonCheckup()
        {
            //Get all crayons in scene
            _tempHolder = GameObject.FindGameObjectsWithTag("Crayon");
            //Get Current Scene
            _currentScene = SceneManager.GetActiveScene().buildIndex;
            //Checks if Scene is already been Checked
            if (_crayonHolder[_currentScene] != null)
            {
                //Check every crayon if they've been previously picked up
                for (int i = 0; i < _tempHolder.Length; i++)
                {
                    //Check if the same indexes ocour
                    if (_crayList[_crayonHolder[_currentScene][i]].IsPicked)
                    {
                        _tempHolder[i].SetActive(false);
                    }
                }
            }
            //If empty fill up list from current scene
            else
            {
                _crayonHolder[_currentScene] = new int[_tempHolder.Length];
                for (int i = 0; i < _tempHolder.Length; i++)
                {
                    _crayList.Add(new CrayonValues(_id, false));
                    _crayonHolder[_currentScene][i] = _id;
                    _id++;
                }
            }
        }
*/
        //<SceneIndex><Int> = nameOfCrayonInScene
        private List<List<string>> savedCrayon;
        
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


