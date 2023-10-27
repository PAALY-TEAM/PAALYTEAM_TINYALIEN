using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TempDisableMovement  tempDisableMovement;

    public SensitivitySettings sensitivitySettings;
    private GameObject thisCanvas;
    private Button resume, settings, reset, exit;
    [SerializeField] private GameObject pausePanel;
    private GameObject thisPanel;
    private int[] savedPlayerStorage;

    private bool isMenuOpen;
    private void Start()
    {
        isMenuOpen = false;
        savedPlayerStorage= new int[ItemManager.numbCarried.Length];
        for (int i = 0; i < ItemManager.numbCarried.Length; i++)
        {
            savedPlayerStorage[i] = ItemManager.numbCarried[i];
        }
        
    }
    private void Update()
    {
        if (isMenuOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isMenuOpen = true;
        //Disable PlayerMovement
        tempDisableMovement.OnPauseGame(true);
        
        thisCanvas = GameObject.Find("CanvasCrayon");
        thisCanvas.GetComponent<Canvas>().sortingOrder = 5;

        thisPanel = Instantiate(pausePanel, thisCanvas.transform.position, Quaternion.identity, thisCanvas.transform);

        // Assign 
        resume = thisPanel.transform.Find("Resume").GetComponent<Button>();
        reset = thisPanel.transform.Find("Restart").GetComponent<Button>();
        settings = thisPanel.transform.Find("Settings").GetComponent<Button>();
        exit = thisPanel.transform.Find("Exit").GetComponent<Button>();
        
        resume.onClick.AddListener(Resume);
        reset.onClick.AddListener(ReloadScene);
        
        exit.onClick.AddListener(Exit);
    }

    private void Resume()
    {
        RemoveListener();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isMenuOpen = false;
        //enable PlayerMovement
        tempDisableMovement.OnResumeGame();
        Destroy(thisPanel);
        
    }

    private void Settings()
    {
        RemoveListener();
    }
    private void ReloadScene()
    {
        RemoveListener();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        for (int i = 0; i < ItemManager.numbStored.Length; i++)
        {
            ItemManager.numbStored[i] = 0;
            ItemManager.numbCarried[i] = savedPlayerStorage[i];
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void RemoveListener()
    {
        resume.onClick.RemoveAllListeners();
        reset.onClick.RemoveAllListeners();
        settings.onClick.RemoveAllListeners(); 
        exit.onClick.RemoveAllListeners();
    }

    private void Exit()
    {
        print("Quit Game");
        Application.Quit();
    }
}
