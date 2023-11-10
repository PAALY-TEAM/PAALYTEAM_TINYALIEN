using TMPro;
using UnityEngine;

namespace Interfaces.ColourChange.ColourChangeScripts
{
    public class SceneSwapActivator : MonoBehaviour, IColourChange
    {
        private GameObject _player;
        private GameObject _panelMade;
    
        //Panel variables
        [SerializeField] private GameObject panelPreFab;
        [SerializeField] private float distanceVisible;
    
        [Header("Values to show playerColor info about minigame")]
        [SerializeField] private string nameOfLevel;
        //HintText variables
        private GameObject _hintText;
        private bool _isActive;
        private bool _isColoured;

        private static float CD;

        private DoorTriggerInteraction _dti;

    

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _dti = GetComponent<DoorTriggerInteraction>();
            Vector3 spawnPlace = new Vector3(0,2f, 0) + transform.position;
            _panelMade = Instantiate(panelPreFab, spawnPlace, Quaternion.identity);
            _panelMade.transform.SetParent(this.gameObject.transform.parent);
        
            _panelMade.transform.Find("Title").GetComponent<TextMeshPro>().text = nameOfLevel;
            _hintText = _panelMade.transform.Find("HintText").gameObject;
            _hintText.SetActive(false);
            _panelMade.SetActive(false);
        }
        public void ColourChange()
        {
            //Activates the script
            _player = GameObject.FindGameObjectWithTag("Player");
            _isActive = false;
            _isColoured = true;
        }

        private void Update()
        {
            if (_isColoured)
            {
                if (Vector3.Distance(_player.transform.position, transform.position) < distanceVisible)
                {
                    _panelMade.SetActive(true);
                    _hintText.SetActive(true);
                    _isActive = true;
                }
                else if (Vector3.Distance(_player.transform.position, transform.position) > distanceVisible &&
                         _isActive)
                {
                    _panelMade.SetActive(false);
                    _hintText.SetActive(false);
                    _isActive = false;
                }

                // Cooldown so the player doesn't return by accident
                if (CD < 0f)
                {
                    CD = 0;
                }
                else if (CD > 0f)
                {
                    CD -= Time.deltaTime;
                }
            }

            // Interact starts the scene swaping
            if (_isActive && Input.GetButtonDown("Interact") && CD <= 0)
            {
                CD = 2f;
                _dti.Interact();
            }
        }
    }
}
