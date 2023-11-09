using Minigame.HideAndSeek;
using TMPro;
using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class MinigameDisplay : MonoBehaviour, IColourChange
    {
        private GameObject _player;
        private Renderer _playerRend;
        private Renderer _rend;
        private GameObject _panelMade;
    
    
        //Panel variables
        [SerializeField] private GameObject panelPreFab;
        [SerializeField] private float distanceVisible;
    
        [Header("Values to show playerColor info about minigame")]
        [SerializeField] private string title;
        [SerializeField] private Sprite reward;
        [SerializeField] private int numbOfReward;
        //HintText variables
        private GameObject _hintText;
        private bool _isActive;

    

        void Start()
        {
        
            _player = GameObject.FindGameObjectWithTag("Player");
            Vector3 spawnPlace = new Vector3(transform.position.x,5f, transform.position.z);
            _panelMade = Instantiate(panelPreFab, spawnPlace, Quaternion.identity);
            _panelMade.transform.SetParent(this.gameObject.transform.parent);
        
            _panelMade.transform.Find("Title").GetComponent<TextMeshPro>().text = title;
            _panelMade.transform.Find("PrizeSprite").GetComponent<SpriteRenderer>().sprite = reward;
            _panelMade.transform.Find("PrizeNumb").GetComponent<TextMeshPro>().text = numbOfReward.ToString();
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
            //Later remove this unique so this script can be used for multiple minigames
            if (_isActive && Input.GetButtonDown("Interact"))
            {
                GameObject.Find("Timer").GetComponent<Timer>().StartHideAndSeek();
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
