using Pickup.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private TempDisableMovement  tempDisableMovement;

        public SensitivitySettings sensitivitySettings;
        private GameObject _thisCanvas;
        private Button _resume, _settings, _reset, _exit;
        [SerializeField] private GameObject pausePanel;
        private GameObject _thisPanel;
        private int[] _savedPlayerStorage;

        private bool _isMenuOpen;
        private void Start()
        {
            _isMenuOpen = false;
            _savedPlayerStorage= new int[ItemManager.NumbCarried.Length];
            for (int i = 0; i < ItemManager.NumbCarried.Length; i++)
            {
                _savedPlayerStorage[i] = ItemManager.NumbCarried[i];
            }
        
        }
        private void Update()
        {
            if (_isMenuOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
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

            // Assign 
            _resume = _thisPanel.transform.Find("Resume").GetComponent<Button>();
            _reset = _thisPanel.transform.Find("Restart").GetComponent<Button>();
            _settings = _thisPanel.transform.Find("Settings").GetComponent<Button>();
            _exit = _thisPanel.transform.Find("Exit").GetComponent<Button>();
        
            _resume.onClick.AddListener(Resume);
            _reset.onClick.AddListener(ReloadScene);
        
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
            RemoveListener();
        }
        private void ReloadScene()
        {
            RemoveListener();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            for (int i = 0; i < ItemManager.NumbStored.Length; i++)
            {
                ItemManager.NumbStored[i] = 0;
                ItemManager.NumbCarried[i] = _savedPlayerStorage[i];
            }
        
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
            print("Quit Game");
            Application.Quit();
        }
    }
}
