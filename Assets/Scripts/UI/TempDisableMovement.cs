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
        private bool _stopInteract;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _playerMovementScript = _player.GetComponent<PlayerMovementV03>();
        }

        public void OnPauseGame(bool stopTime)
        {
            if (!stopTime)
                return;
            Time.timeScale = 0;
            _player.GetComponent<ItemManager>().menuOpen = true;
            _playerMovementScript.enabled = false;
        }

        public void OnResumeGame()
        {
            Time.timeScale = 1;
            _playerMovementScript.enabled = true;
            _player.GetComponent<ItemManager>().menuOpen = false;
        }
    }
}
