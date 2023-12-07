using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        Instantiate(playerPrefab);
    }
}
