using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{
    public string sceneToLoad;
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        // Check if the player presses the interact button
        if (Input.GetKeyDown(interactKey))
        {
            // If the player is inside the trigger, load the new scene
            if (playerInsideTrigger)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private bool playerInsideTrigger = false;

    private void OnTriggerStay(Collider other)
    {
        // Check if the object inside the trigger has the player tag
        if (other.CompareTag(playerTag))
        {
            playerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger has the player tag
        if (other.CompareTag(playerTag))
        {
            playerInsideTrigger = false;
        }
    }
}