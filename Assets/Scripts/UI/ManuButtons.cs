using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ManuButtons : MonoBehaviour
    {
        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
