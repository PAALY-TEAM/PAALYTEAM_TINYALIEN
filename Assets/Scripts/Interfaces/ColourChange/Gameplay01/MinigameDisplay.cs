using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using TMPro;
using UnityEngine;

public class MinigameDisplay : MonoBehaviour, IColourChange
{
    private GameObject player;
    private Renderer playerRend;
    private Renderer rend;
    private GameObject panelMade;
    
    
    //Panel variables
    [SerializeField] private GameObject panelPreFab;
    [SerializeField] private float distanceVisible;
    
    [Header("Values to show playerColor info about minigame")]
    [SerializeField] private string title;
    [SerializeField] private Sprite reward;
    [SerializeField] private int numbOfReward;
    //HintText variables
    private GameObject hintText;
    private bool isActive;

    

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 spawnPlace = new Vector3(transform.position.x,5f, transform.position.z);
        panelMade = Instantiate(panelPreFab, spawnPlace, Quaternion.identity);
        panelMade.transform.SetParent(this.gameObject.transform.parent);
        
        panelMade.transform.Find("Title").GetComponent<TextMeshPro>().text = title;
        panelMade.transform.Find("PrizeSprite").GetComponent<SpriteRenderer>().sprite = reward;
        panelMade.transform.Find("PrizeNumb").GetComponent<TextMeshPro>().text = numbOfReward.ToString();
        hintText = panelMade.transform.Find("HintText").gameObject;
        hintText.SetActive(false);
        panelMade.SetActive(false);
    }

    private bool isColoured;
    public void ColourChange()
    {
        //Activates the script
        player = GameObject.FindGameObjectWithTag("Player");
        playerRend = player.transform.Find("AlienBody_Floating").GetComponent<Renderer>();
        rend = GetComponent<Renderer>();
        isActive = false;
        isColoured = true;
       
    }

    
    private void Update()
    {
        if (isColoured)
        {
            if (Vector3.Distance(player.transform.position, transform.position ) < distanceVisible)
            {
                panelMade.SetActive(true);
                hintText.SetActive(true);
                isActive = true;
            }
            else if (Vector3.Distance(player.transform.position, transform.position)  > distanceVisible && isActive)
            {
                panelMade.SetActive(false);
                hintText.SetActive(false);
                isActive = false;
            }
        }
        //Later remove this unique so this script can be used for multiple minigames
        if (isActive && Input.GetButtonDown("Interact"))
        {
            GameObject.Find("Timer").GetComponent<Timer>().StartHideAndSeek();
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (isActive && rend.sharedMaterial == playerRend.sharedMaterial && other.gameObject.CompareTag("Player"))
        {
            hintText.SetActive(true);
        } 
    }
    private void OnCollisionExit(Collision other)
    {
        if (isActive && other.gameObject.CompareTag("Player"))
        {
            hintText.SetActive(false);
        }
    }
}
