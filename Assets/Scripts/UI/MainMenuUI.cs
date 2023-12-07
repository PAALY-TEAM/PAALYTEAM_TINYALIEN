using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //very first based on CodeMonkey kitchen chaos tutorial
    //then rewritten to suit logic from the MarieUI script
    public sealed class MainMenuUI : MonoBehaviour {
        
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button helpButton;
        [SerializeField] private Button quitButton;
        
        private void Awake() {
            playButton.onClick.AddListener(() => {
                Loader.Load(Loader.Scene.GameScene);
            });
            settingsButton.onClick.AddListener(() => {
                Loader.Load(Loader.Scene.Settings);
            });
            helpButton.onClick.AddListener(() => {
                Loader.Load(Loader.Scene.HelpScene);
            });
            quitButton.onClick.AddListener(Quit);

            //ensure that the game returns to normal speed when the main menu is loaded
            Time.timeScale = 1f;
        }
        //copy-pasted from Feel MMApplicationQuit.cs
        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}