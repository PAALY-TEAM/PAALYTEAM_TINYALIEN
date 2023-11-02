using System.Collections;
using System.Collections.Generic;
using Pickup.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoader : MonoBehaviour
{
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>().MySceneLoader();
    }
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>().MySceneLoader();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
