using System;
using Pickup;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.TextActivators
{
    public class WhenCollectedCrayons : MonoBehaviour
    {
        [Header("Number of crayons to activate dialogue")]
        [SerializeField] private int[] numbCrayonsToActivate;
        [SerializeField] private GameObject[] dialogueSummoner;
        private static bool[] _isUsed;

        private void Awake()
        {
            _isUsed = new bool[dialogueSummoner.Length];
        }

        public void CheckIfEnough()
        {
            for (int i = numbCrayonsToActivate.Length-1; i >= 0; i--)
            {
                if (numbCrayonsToActivate[i] <= GetComponent<ShipInventory>().p && !_isUsed[i])
                {
                    dialogueSummoner[i].GetComponent<NpcTextBox>().DialogueStart();
                    _isUsed[i] = true;
                    return;
                }
            }
            dialogueSummoner[^1].GetComponent<NpcTextBox>().DialogueStart();
        }
    }
}
