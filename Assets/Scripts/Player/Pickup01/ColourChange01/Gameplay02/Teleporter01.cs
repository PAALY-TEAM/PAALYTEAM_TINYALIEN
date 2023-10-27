using UnityEngine;

namespace Pickup01
{
    public class Teleporter01 : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject hintText;
        [Header("Destination to teleport")]
        [SerializeField] private GameObject currentScene;
        [SerializeField] private GameObject nextScene;
        private bool isActive = false;
        private bool isClose;

        private Renderer rend;
        [SerializeField] private Renderer playerRend;
    
        public void ColourChange()
        {
            //Activates the script
            isActive = true;
            rend = GetComponent<Renderer>();
        }

        private void OnCollisionStay(Collision other)
        {
            if (isActive && rend.sharedMaterial == playerRend.sharedMaterial)
            {
                hintText.SetActive(true);
                isClose = true;
            } else {
                hintText.SetActive(false);
                isClose = false;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (isActive)
            {
                hintText.SetActive(false);
                isClose = false;
            }
        }


        private void Update()
        {
            if (isActive && isClose && Input.GetButtonDown("Interact"))
            {
                nextScene.SetActive(true); 
                currentScene.SetActive(false);
                hintText.SetActive(false);
                isClose = false;
                player.GetComponent<ItemManager01>().MySceneLoader();
            }
        }

    
    }
}
