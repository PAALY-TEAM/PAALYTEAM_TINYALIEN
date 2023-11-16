using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour {


        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button helpButton;
        [SerializeField] private Button quitButton;


        private void Awake() {
            playButton.onClick.AddListener(() => {
                Loader.Load(Loader.Scene.GameScene);
            });
            settingsButton.onClick.AddListener(() => {
                
            });
            helpButton.onClick.AddListener(() => {
                
            });
            quitButton.onClick.AddListener(() => {
                Application.Quit();
            });

            //ensure that the game returns to normal speed when the main menu is loaded
            Time.timeScale = 1f;
        }

    }
}