using Pickup;
using UnityEngine;

namespace UI.TextActivators
{
    public class WhenCollectedCrayons : MonoBehaviour
    {
        [Header("Number of crayons to activate dialogue")]
        [SerializeField] private int numbCrayonsToActivate;

        private bool _isUsed = false;

        public void CheckIfEnough()
        {
            if (numbCrayonsToActivate <= GetComponent<ShipInventory>().p && !_isUsed)
            {
                transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
                _isUsed = true;
            
            }
        }
    }
}
