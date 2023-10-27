using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextOnStart : MonoBehaviour
{
    private static bool isReloaded = true;

    private void Awake()
    {
        if (isReloaded)
        {
            isReloaded = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
    }

    void Start()
    {
        transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
    }
}
