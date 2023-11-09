using UI;
using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class Narate : MonoBehaviour, IColourChange

    {
        private bool used;
        public void ColourChange()
        {
            if (transform.Find("DialogueSummoner") && !used)
            {
                transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
                used = true;
            }
            
        }
    }
}
