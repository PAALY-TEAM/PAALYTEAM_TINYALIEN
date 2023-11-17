using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Player;
using UI;
using UnityEngine;

public class LoseGame : MonoBehaviour
{
    [Header("Add PanelSummoner with lose text named LoseText")]
    private CrayonLost _crayonLost;

    private void Start()
    {
        _crayonLost = GameObject.Find("CrayonLost").GetComponent<CrayonLost>();
    }

    public void Lose(Vector3[] crayonSpawns, Vector3 playerSpawn)
    {
        var playerScript = GetComponent<ItemManager>();
        //Script to make playerColor lose crayon on lose
        var playerCrayons = ItemManager.NumbCarried;
        for (var i = 0; i < playerCrayons.Length; i++)
        {
            if (playerCrayons[i] > 0)
            {
                playerCrayons[i]--;
				
                playerScript.CrayonProgress--;
                playerScript.UpdateValues();
                // Create Crayon and add to list of stolen
                _crayonLost.AddLostCrayon(i, crayonSpawns);
					
                transform.Find("LoseText").GetComponent<NpcTextBox>().DialogueStart();
                break;
            }
        }
        //Move Player
        playerScript.MoveAlien(playerSpawn);
    }
}
