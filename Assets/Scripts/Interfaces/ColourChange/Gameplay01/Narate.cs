using UI;
using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class Narate : MonoBehaviour, IColourChange
    {
        public void ColourChange()
        {
            transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
        }
    }
}
