using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;



public class NpcTextBox : MonoBehaviour
{
    private GameObject thisCanvas;
        [Header("Panel 0: Only Text")] 
        [Header("Panel 1: Text and Image")]
        [SerializeField] private GameObject[] panelPrefab;
            [SerializeField] private Sprite npcImg;
                [SerializeField] private string[] npcName;
                [SerializeField] private GameObject[] cameraFocus;
            [SerializeField] private string[] textOnPanel;
            private int currentText;
            [SerializeField] private Sprite[] imgOnPanel;
            private int currentImg;
            private Button prev,next;

    private CinemachineVirtualCamera cam;
            
    [Header("Order the type of panels appear in, write only numbers (0 or 1)")] 
    [SerializeField] private string panelsToSpawn;
    private int[] order;
    [Header("Disables movement of playerColor and camera")]
    [SerializeField] private TempDisableMovement  tempDisableMovement;
    
    private int sumPages;
    private int currentPage;
    private int currentName;
    private GameObject thisPage;
    private GameObject menu;

    private bool activeDialogue;
    

    public void DialogueStart()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menu = GameObject.Find("PauseSummoner");
        menu.SetActive(false);
        
        
        activeDialogue = true;
        //Finds Camera In Scene so that it can swap focus during scenes  
        cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        
        thisCanvas = GameObject.Find("CanvasCrayon");
        currentText = 0;
        currentImg = 0;
        currentPage = 0;
        sumPages = textOnPanel.Length;
        order = new int[panelsToSpawn.Length];
        for (int  i = 0;  i < panelsToSpawn.Length;  i++)
        {
            order[i] = int.Parse(panelsToSpawn[i].ToString());
        }
        
        tempDisableMovement.OnPauseGame(order[0]!=2);
        DialogueContinue();
    }

    private void DialogueContinue()
    {
        if (panelPrefab.Length > 0)
        {
            thisPage = Instantiate(panelPrefab[order[currentPage]], thisCanvas.transform.position, 
                Quaternion.identity, thisCanvas.transform);
            //Panel with only text
            if (order[currentPage] == 0)
            {
                thisPage.transform.Find("Profile").GetComponent<Image>().sprite = npcImg;
                thisPage.transform.Find("Profile/Name").GetComponent<TextMeshProUGUI>().text = npcName[0];
                thisPage.transform.Find("MainText").GetComponent<TextMeshProUGUI>().text = textOnPanel[currentText];
            } 
            //Panel with text and Image
            else if (order[currentPage] == 1)
            {
                thisPage.transform.Find("MainText").GetComponent<TextMeshProUGUI>().text = textOnPanel[currentText];
                thisPage.transform.Find("MainImage").GetComponent<Image>().sprite = imgOnPanel[currentImg];
            }
            //Smaller panel for quick and shorter dialogue
            else if (order[currentPage] == 2)
            {
                thisPage.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
                thisPage.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                thisPage.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = npcName[currentText];
                thisPage.transform.Find("MainText").GetComponent<TextMeshProUGUI>().text = textOnPanel[currentText];
                cam.LookAt = cameraFocus[currentText].transform;
                cam.Follow = cameraFocus[currentText].transform;
                
                
                return;
            }
        }
        //Find New Buttons
        prev = thisPage.transform.Find("Prev").GetComponent<Button>();
        next = thisPage.transform.Find("Next").GetComponent<Button>();
        // Add new listeners
        prev.onClick.AddListener(PrevPanel);
        next.onClick.AddListener(NextPanel);
        
        if (currentPage == 0)
        {
            prev.interactable = false; 
        }
        if (currentPage + 1 == sumPages)
        {
            next.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Exit";
        }
    }

    private void Update()
    {
        if (activeDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DialogueEnd();
            }

            if (Input.GetButtonDown("Interact") && order[currentPage] == 2)
            {
                NextPanel();
            }
        }
    }

    private void DialogueEnd()
    {
        print("Ended Dialogue");
        Destroy(thisPage);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeDialogue = false;
        menu.SetActive(true);
        tempDisableMovement.OnResumeGame();
        
        //Set so player is the focus of camera
        var camFocus = GameObject.FindGameObjectWithTag("Player").transform.Find("CameraTarget").transform;
        print(camFocus);
        cam.LookAt = camFocus;
        cam.Follow = camFocus;
    }
    private void PrevPanel()
    { 
        SwapPanel(-1);
    }
    private void NextPanel()
    {
        SwapPanel(1);
    }

    private void SwapPanel(int dir)
    {
        //Only if image was used
        if (currentPage != 0)
        {
            if ((order[currentPage] == 1 && dir == 1)||(order[currentPage-1] == 1 && dir == -1))
            {
                currentImg += dir;
            }
        }
        
        // Because dialogue panel has no button it will ignore this part 
        if (order[currentPage] != 2)
        {
            // Remove previous listeners
            prev.onClick.RemoveAllListeners();
            next.onClick.RemoveAllListeners();
        }
        currentText += dir;
        currentPage += dir;
        
        
        
        if (currentPage == sumPages)
        {
            DialogueEnd();
            print("DialogueEndCaled");
        } else {
            Destroy(thisPage);
            DialogueContinue();
        }
    }
}
