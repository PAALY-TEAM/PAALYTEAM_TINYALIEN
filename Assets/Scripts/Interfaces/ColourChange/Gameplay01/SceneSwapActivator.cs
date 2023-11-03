using TMPro;
using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class SceneSwapActivator : MonoBehaviour, IColourChange
    {
        private GameObject _player;
        private Renderer _playerRend;
        private Renderer _rend;
        private GameObject _panelMade;
    
        //Panel variables
        [SerializeField] private GameObject panelPreFab;
        [SerializeField] private float distanceVisible;
    
        [Header("Values to show playerColor info about minigame")]
        [SerializeField] private string nameOfLevel;
        //HintText variables
        private GameObject _hintText;
        private bool _isActive;

        private DoorTriggerInteraction _DTI;

    

        void Start()
        {
        
            _player = GameObject.FindGameObjectWithTag("Player");
            _DTI = GetComponent<DoorTriggerInteraction>();
            Vector3 spawnPlace = new Vector3(0,2f, 0) + transform.position;
            _panelMade = Instantiate(panelPreFab, spawnPlace, Quaternion.identity);
            _panelMade.transform.SetParent(this.gameObject.transform.parent);
        
            _panelMade.transform.Find("Title").GetComponent<TextMeshPro>().text = nameOfLevel;
            _hintText = _panelMade.transform.Find("HintText").gameObject;
            _hintText.SetActive(false);
            _panelMade.SetActive(false);
            
        }

        private bool _isColoured;
        public void ColourChange()
        {
            //Activates the script
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerRend = _player.transform.Find("AlienBody_Floating").GetComponent<Renderer>();
            _rend = GetComponent<Renderer>();
            _isActive = false;
            _isColoured = true;
       
        }

    
        private void Update()
        {
            if (_isColoured)
            {
                if (Vector3.Distance(_player.transform.position, transform.position ) < distanceVisible)
                {
                    _panelMade.SetActive(true);
                    _hintText.SetActive(true);
                    _isActive = true;
                }
                else if (Vector3.Distance(_player.transform.position, transform.position)  > distanceVisible && _isActive)
                {
                    _panelMade.SetActive(false);
                    _hintText.SetActive(false);
                    _isActive = false;
                }
            }
            // Interact starts the scene swaping
            if (_isActive && Input.GetButtonDown("Interact"))
            {
                _DTI.Interact();
            }
        }
        private void OnCollisionStay(Collision other)
        {
            if (_isActive && _rend.sharedMaterial == _playerRend.sharedMaterial && other.gameObject.CompareTag("Player"))
            {
                _hintText.SetActive(true);
            } 
        }
        private void OnCollisionExit(Collision other)
        {
            if (_isActive && other.gameObject.CompareTag("Player"))
            {
                _hintText.SetActive(false);
            }
        }
    }
}
