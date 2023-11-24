using System;
using Camera;
using Movement;
using Pickup.Player;
using UnityEngine;

namespace UI
{
    public class TempDisableMovement : MonoBehaviour
    {
        private MonoBehaviour _playerMovementScript;

        private GameObject _player;

        private bool _freeze;

        private bool StopInteract;
        //private MonoBehaviour orbitCameraScript;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _playerMovementScript = _player.GetComponent<PlayerMovementV03>();
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
                _player.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezePosition;
            }

            _player.GetComponent<ItemManager>().menuOpen = true;
            _playerMovementScript.enabled = false;
            //orbitCameraScript.enabled = false;
        }

        public void OnResumeGame()
        {
            Time.timeScale = 1;
            _playerMovementScript.enabled = true;
            _player.GetComponent<ItemManager>().menuOpen = false;
            _player.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.None;
            //orbitCameraScript.enabled = true;
        }
    }
}