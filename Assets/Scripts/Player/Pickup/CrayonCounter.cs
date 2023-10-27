using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrayonCounter : MonoBehaviour
{
    private bool isFirstTime = true;
    
    
    private Scene[] ScenesInBuild;

    //[Current Scene Index][Index to crayon in scene]
    private int[][] crayonHolder;

    GameObject[] tempHolder;
    private int currentScene;
    
    List<CrayonValues> crayList = new List<CrayonValues>();
    private int ID = 0;

    private void Awake()
    {
        if (isFirstTime)
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            ScenesInBuild = new Scene[sceneCount];
            crayonHolder = new int[sceneCount][];

            //Go through specific scenes for when we have multiple levels/worlds
            
            for (int i = 0; i < sceneCount; i++)
            {
                ScenesInBuild[i] = SceneManager.GetSceneByBuildIndex(i);
            }
            crayList.Clear();

            isFirstTime = false;
        }
        
    }

    public void CrayonCheckup()
    {
        //Get all crayons in scene
        tempHolder = GameObject.FindGameObjectsWithTag("Crayon");
        //Get Current Scene
        currentScene = SceneManager.GetActiveScene().buildIndex;
        //Checks if Scene is already been Checked
        if (crayonHolder[currentScene] != null)
        {
            //Check every crayon if they've been previously picked up
            for (int i = 0; i < tempHolder.Length; i++)
            {
                //Check if the same indexes ocour
                if (crayList[crayonHolder[currentScene][i]].IsPicked)
                {
                    tempHolder[i].SetActive(false);
                }
            }
        }
        //If empty fill up list from current scene
        else
        {
            crayonHolder[currentScene] = new int[tempHolder.Length];
            for (int i = 0; i < tempHolder.Length; i++)
            {
                crayList.Add(new CrayonValues(ID, false));
                crayonHolder[currentScene][i] = ID;
                ID++;
            }
        }
    }

    public void ChangeThisCrayonStatus(GameObject thisCrayon)
    {
        
        var activeScene = SceneManager.GetActiveScene().buildIndex;
        for (int i = 0; i < tempHolder.Length; i++)
        {
            if (tempHolder[i] == thisCrayon)
            {
                //Finds thisCrayon in the list were all crayons are stored and identify them as picked up 
                crayList.Find(x => x.Crayon == crayonHolder[currentScene][i]).IsPicked=true;
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


