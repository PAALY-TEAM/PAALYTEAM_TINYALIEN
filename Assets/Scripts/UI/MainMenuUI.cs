using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class MainMenuUI : MonoBehaviour {

        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button helpButton;
        [SerializeField] private Button quitButton;
        
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject helpPanel;

        private void Awake() {
            playButton.onClick.AddListener(() => {
                Loader.Load(Loader.Scene.GameScene);
            });
            settingsButton.onClick.AddListener(OpenSettings);
            helpButton.onClick.AddListener(OpenHelp);
            quitButton.onClick.AddListener(Quit);

            Time.timeScale = 1f;
        }

        private void OpenSettings()
        {
            settingsPanel.SetActive(true);
        }

        private void OpenHelp()
        {
            helpPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            settingsPanel.SetActive(false);
        }

        public void CloseHelp() 
        {
            helpPanel.SetActive(false);
        }

        private static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}