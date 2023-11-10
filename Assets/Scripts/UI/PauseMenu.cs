using System;
using System.Collections.Generic;
using Pickup;
using Pickup.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private TempDisableMovement  tempDisableMovement;
        private ItemManager _itemManager;

        public SensitivitySettings sensitivitySettings;
        private GameObject _thisCanvas;
        private Button _resume, _settings, _reset, _exit;
        [SerializeField] private GameObject pausePanel;
        private GameObject _thisPanel;
        private GameObject _player;
        //Values to save for restart button
        private int[] _savedShipStorage;
        private int[] _savedPlayerStorage;
        private List<string> _savedCrayonCounter;
        private int _savedCurrentColour;
        private Vector3 _savedPos;
        
        private CrayonCounter _crayonCounter;

        private bool _isMenuOpen = false;

        private TextMeshProUGUI crayonsLeft;

        public bool inDialogue;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _itemManager = _player.GetComponent<ItemManager>();
            _crayonCounter = GameObject.Find("CrayonCounter").GetComponent<CrayonCounter>();
        }

        //Set new value for current scene, run by ItemManager MySceneLoader();
        public void NewValues()
        {
            _savedShipStorage = new int[ItemManager.NumbStored.Length];
            _savedPlayerStorage = new int[ItemManager.NumbCarried.Length];
            _savedCrayonCounter = new List<string>();
            _savedCrayonCounter = _crayonCounter.savedCrayon[SceneManager.GetActiveScene().buildIndex];
            for (int i = 0; i < ItemManager.NumbCarried.Length; i++)
            {
                
                _savedPlayerStorage[i] = ItemManager.NumbCarried[i];
                if (i < ItemManager.NumbStored.Length)
                {
                    _savedShipStorage[i] = ItemManager.NumbStored[i];
                }
            }

            _savedCurrentColour = _itemManager.currentColour;
            _savedPos = _player.transform.position;
            print("NewValues Ran");
        }
        private void Update()
        {
            if (_isMenuOpen && Input.GetButtonDown("Pause"))
            {
                Resume();
            }
            else if (Input.GetButtonDown("Pause"))
            {
                Pause();
            }
        }
        private void Pause()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _isMenuOpen = true;
            //Disable PlayerMovement
            tempDisableMovement.OnPauseGame(true);
        
            _thisCanvas = GameObject.Find("CanvasCrayon");
            _thisCanvas.GetComponent<Canvas>().sortingOrder = 5;

            _thisPanel = Instantiate(pausePanel, _thisCanvas.transform.position, Quaternion.identity, _thisCanvas.transform);

            // Find crayons in scene and display for player
            int crayonInScene = GameObject.FindGameObjectsWithTag("Crayon").Length;
            crayonsLeft = _thisPanel.transform.Find("CL").GetComponent<TextMeshProUGUI>();
            crayonsLeft.text = "Crayons Left In This Level: " + crayonInScene;
            // Assign 
            _resume = _thisPanel.transform.Find("Resume").GetComponent<Button>();
            _reset = _thisPanel.transform.Find("Restart").GetComponent<Button>();
            _settings = _thisPanel.transform.Find("Settings").GetComponent<Button>();
            _exit = _thisPanel.transform.Find("Exit").GetComponent<Button>();
        
            _resume.onClick.AddListener(Resume);
            _reset.onClick.AddListener(ReloadScene);
            _settings.onClick.AddListener(Settings);
            _exit.onClick.AddListener(Exit);
        }

        private void Resume()
        {
            RemoveListener();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isMenuOpen = false;
            //enable PlayerMovement
            tempDisableMovement.OnResumeGame();
            Destroy(_thisPanel);
        
        }

        private void Settings()
        {
            //RemoveListener();
        }
        private void ReloadScene()
        {
            //Gotta reset the crayons picked up value also smh
            RemoveListener();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            // Sets the player inventory to what it was when entering scene
            for (int i = 0; i < ItemManager.NumbStored.Length; i++)
            {
                ItemManager.NumbStored[i] = _savedShipStorage[i];
                ItemManager.NumbCarried[i] = _savedPlayerStorage[i];
            }
            _crayonCounter.ReloadFunc(_savedCrayonCounter);
            _itemManager.MovePlayer(_savedPos);
            _itemManager.currentColour = _savedCurrentColour;
            _itemManager.ChangeAlienColour(_savedCurrentColour);
            
            Destroy(_thisPanel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void RemoveListener()
        {
            _resume.onClick.RemoveAllListeners();
            _reset.onClick.RemoveAllListeners();
            _settings.onClick.RemoveAllListeners(); 
            _exit.onClick.RemoveAllListeners();
        }

        private void Exit()
        {
            RemoveListener();
        }
    }
}
