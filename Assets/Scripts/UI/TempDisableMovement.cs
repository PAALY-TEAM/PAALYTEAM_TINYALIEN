using System;
using Camera;
using Movement;
using UnityEngine;

namespace UI
{
    public class TempDisableMovement : MonoBehaviour
    {
        private MonoBehaviour playerMovementScript;

        private GameObject player;

        private bool freeze;
        //private MonoBehaviour orbitCameraScript;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            playerMovementScript = player.GetComponent<PlayerMovementV03>();
            //orbitCameraScript = GameObject.Find("Virtual Camera").GetComponent<NewOrbitCamera>();
        }

        

        public void OnPauseGame(bool stopTime)
        {
            if (stopTime)
            {
                Time.timeScale = 0; 
            }
            else
            {
                player.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezePosition;
            }
            playerMovementScript.enabled = false;
            //orbitCameraScript.enabled = false;
        }

        public void OnResumeGame()
        {
            Time.timeScale = 1;
            playerMovementScript.enabled = true;
            player.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.None;
            //orbitCameraScript.enabled = true;
        }
    }
}