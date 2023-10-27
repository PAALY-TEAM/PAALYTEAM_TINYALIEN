using System.Collections;
using System.Collections.Generic;
using Pickup.Player;
using UnityEngine;

public class StartLoader : MonoBehaviour
{
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>().MySceneLoader();
    }
}
