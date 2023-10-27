using Pickup.Player;
using SceneLogic;
using UnityEngine;

namespace Interaction
{
    public class DoorTriggerInteraction : TriggerInteractionBase
    {
        // Which door is this?
        public enum DoorToSpawnAt
        {
            None,
            One,
            Two,
            Three,
            Four,
            Five,
            // Add more if needed
        }

        [Header("Spawn To")]
        [SerializeField] private DoorToSpawnAt DoorToSpawnTo; // Which door in new scene are we going to go into
        [SerializeField] private SceneField _sceneToLoad;

        [Space(10f)]
        [Header("THIS DOOR")]
        public DoorToSpawnAt CurrentDoorPosition;

        [Header("Interaction")]
        public string playerTag = "Player";
        public KeyCode interactKey = KeyCode.E;

        private bool playerInsideTrigger = false;

        private void Update()
        {
            if (playerInsideTrigger && Input.GetKeyDown(interactKey))
            {
                Interact();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInsideTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                playerInsideTrigger = false;
            }
        }

        public override void Interact()
        {
            // Load new scene
            GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>().hintText.SetActive(true);
            SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
        }
    }
}