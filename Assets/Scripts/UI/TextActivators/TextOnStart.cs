using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.TextActivators
{
    public class TextOnStart : MonoBehaviour
    {
        void Start()
        {
            transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
        }
    }
}
