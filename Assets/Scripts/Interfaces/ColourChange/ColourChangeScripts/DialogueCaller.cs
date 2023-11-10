using UI;
using UnityEngine;

namespace Interfaces.ColourChange.Gameplay01
{
    public class DialogueCaller : MonoBehaviour, IColourChange
    {
        private bool _isColoured = false;
        public void ColourChange()
        {
            if (!_isColoured)
                transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();

            _isColoured = true;
        }
    }
}
