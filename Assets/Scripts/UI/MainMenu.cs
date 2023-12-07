using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("GameScene");
        }
        public void Settings()
        {
            SceneManager.LoadScene("Settings");
        }

        public void MainMenu2()
        {
            SceneManager.LoadScene("MainMenu2");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        public void Help()
        {
            SceneManager.LoadScene("HelpScene");
        }
    }
}
