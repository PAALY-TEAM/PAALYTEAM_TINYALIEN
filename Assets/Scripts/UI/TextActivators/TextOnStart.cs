using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.TextActivators
{
    public class TextOnStart : MonoBehaviour
    {
        private static bool _isReloaded = true;

        private void Awake()
        {
            if (_isReloaded)
            {
                _isReloaded = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            }
        }

        void Start()
        {
            transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
        }
    }
}
