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
        _crayonLost = GameObject.Find("CrayonCounter").GetComponent<CrayonLost>();
    }

    public void Lose(Transform[] crayonSpawns, Vector3 playerSpawn)
    {
        Vector3[] crayonSpawnsVectors = new Vector3[crayonSpawns.Length];
        for (int i = 0; i < crayonSpawnsVectors.Length; i ++)
        {
            crayonSpawnsVectors[i] = crayonSpawns[i].position;
        }
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
                _crayonLost.AddLostCrayon(i, crayonSpawnsVectors);
					
                GameObject.Find("LoseText").GetComponent<NpcTextBox>().DialogueStart();
                break;
            }
        }
        //Move Player
        playerScript.MoveAlien(playerSpawn);
    }
}
