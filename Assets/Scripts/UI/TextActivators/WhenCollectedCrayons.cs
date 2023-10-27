using System;
using System.Collections;
using System.Collections.Generic;
using Pickup;
using UnityEngine;

public class WhenCollectedCrayons : MonoBehaviour
{
    [Header("Number of crayons to activate dialogue")]
    [SerializeField] private int numbCrayonsToActivate;

    private bool isUsed = false;

    public void CheckIfEnough()
    {
        if (numbCrayonsToActivate <= GetComponent<ShipInventory>().p && !isUsed)
        {
            transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
            isUsed = true;
            
        }
    }
}
