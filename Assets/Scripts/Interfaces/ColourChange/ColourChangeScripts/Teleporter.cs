using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class Teleporter : MonoBehaviour
    {
        public GameObject playerColor;
        [SerializeField] private Material targetMaterial;
        [SerializeField] private GameObject hintText;
        [Header("Destination to teleport")]
        [SerializeField] private GameObject currentScene;
        [SerializeField] private GameObject nextScene;
        private bool _isClose;

        private Renderer _rend;
        private Renderer _playerRenderer;

        private void Start()
        {
            _rend = GetComponent<Renderer>();
            playerColor = GameObject.FindGameObjectWithTag("PlayerColor");
            _playerRenderer = playerColor.GetComponent<Renderer>();
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.CompareTag("PlayerColor"))
            {
                if (_playerRenderer.sharedMaterial == targetMaterial)
                {
                    hintText.SetActive(true);
                    _isClose = true;
                }
                else
                {
                    hintText.SetActive(false);
                    _isClose = false;
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("PlayerColor"))
            {
                hintText.SetActive(false);
                _isClose = false;
            }
        }

        private void Update()
        {
            if (_isClose && Input.GetButtonDown("Interact"))
            {
                nextScene.SetActive(true);
                currentScene.SetActive(false);
                hintText.SetActive(false);
                _isClose = false;
            }
        }
    }
}