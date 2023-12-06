using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLose : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("If problem with scene swap check the name of scene in code");
            SceneManager.LoadScene("02_GameSceneDuplicate");
            var itemManager = other.GetComponent<ItemManager>();
            var spawnPos = GameObject.FindWithTag("RespawnPoint").transform.position;
            itemManager.MoveAlien(spawnPos);
        }
    }
}
