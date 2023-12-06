using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class CrayonsCollected : MonoBehaviour
{
    public void CheckIfAnyLeft()
    {
        
        // For some reason it will still count the just destroyed GameObject
        if (NumbOfCrayonsInScene() <= 1)
        {
            transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
        }
    }

    // return number of crayons in scene
    public static int NumbOfCrayonsInScene()
    {
        //Check if the GameObject with crayons exist
        if (GameObject.Find("CrayonHolder") != null)
        {
            int crayonsInScene = 0;
            // Get transform of crayon to check on child
            Transform crayonHolder = GameObject.Find("CrayonHolder").transform;
            foreach (Transform crayon in crayonHolder)
            {
                //Making sure to only check on active as already picked up is inactive
                if (crayon.gameObject.activeSelf)
                {
                    crayonsInScene++;
                }
            }
        
            return crayonsInScene;
        }
        Debug.Log("no GameObject named CrayonHolder in scene, returns 0");
        return 0;
    }
}
