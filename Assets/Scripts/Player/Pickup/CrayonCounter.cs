using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pickup
{
    public class CrayonCounter : MonoBehaviour
    {
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

        public void ChangeThisCrayonStatus(GameObject thisCrayon)
        {
        
            var activeScene = SceneManager.GetActiveScene().buildIndex;
            for (int i = 0; i < _tempHolder.Length; i++)
            {
                if (_tempHolder[i] == thisCrayon)
                {
                    //Finds thisCrayon in the list were all crayons are stored and identify them as picked up 
                    _crayList.Find(x => x.Crayon == _crayonHolder[_currentScene][i]).IsPicked=true;
                    break;
                }
            }
        }
        class CrayonValues
        {
            public int Crayon;
            public bool IsPicked;
            public CrayonValues(int crayon, bool isPicked)
            {
                this.Crayon = crayon;
                this.IsPicked = isPicked;
            } 
        }
    }
}


