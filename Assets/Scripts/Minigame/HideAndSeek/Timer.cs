using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Time the player has to hide and another for how long the game should last")]
    [SerializeField] private float insertHideTime;
    [SerializeField] private float insertPlayTime;
    private float timeToHide;
    private float timeToPlay;
    private TextMeshProUGUI timer;

    [SerializeField] private GameObject seekers;
    
    
    [HideInInspector] public bool gameOver = false;
    private bool gameStart = false;

    [SerializeField] private Vector3 EndPos;

    private GameObject guard;
    private GameObject crayon;

    [SerializeField] private GameObject crayonPrefab;
    [SerializeField] private Vector3[] crayonInstPlace;
    
    
    private void Start()
    {
        guard = GameObject.Find("Guards");
        crayon = GameObject.Find("Crayons");
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            gameOver = true;
        }

        if (gameStart)
        {
            bool activeSeeker = false;
            if (timeToHide > 0)
            {
                //Decreases timer and displays whole number
                timeToHide -= Time.deltaTime;
                var currentTime = timeToHide.ToString("F0");
                timer.text = currentTime;
            }
            else if (timeToPlay > 0)
            {
                if (!activeSeeker)
                {
                    GangComponents(true);
                    activeSeeker = true;
                }
                //Decreases timer and displays whole number
                timeToPlay -= Time.deltaTime;
                var currentTime = timeToPlay.ToString("F0");
                timer.text = currentTime;

                if (gameOver)
                {
                    Lose();
                    
                    timeToPlay = 0;
                }
            }
            else
            {
                Win();
            }
        }

    }
    public void StartHideAndSeek()
    {
        //Get timer component
        timer = GetComponent<TextMeshProUGUI>();
        //Initialize timers
        timeToHide = insertHideTime;
        timeToPlay = insertPlayTime;
        //TempRemove Guards & Crayons
        guard.SetActive(false);
        crayon.SetActive(false);
        //StartsGame when everything is initialized
        gameStart = true;
    }

    private void Win()
    {
        //Could spawn Victory or game completion screen
        gameStart = false;
        ReturnToStart(true);
    }
    private void Lose()
    {
        //Could Spawn in lose screen with retry and exit button
        gameOver = false;
        gameStart = false;
        ReturnToStart(false);
    }

    private void ReturnToStart(bool won)
    {
        GangComponents(false);
        
        GameObject.FindGameObjectWithTag("Player").transform.position = EndPos;
        Physics.SyncTransforms();
        //Bring back Guards and Crayons
        guard.SetActive(true);
        crayon.SetActive(true);
        if (won)
        {
            foreach (var pos in crayonInstPlace)
            {
                Instantiate(crayonPrefab, pos, Quaternion.identity);
            }
        }
    }

    private void GangComponents(bool b)
    {
        for (int i = 0; i < 4; i++)
        {
            var currentChild = seekers.transform.GetChild(i);
            currentChild.GetComponent<Light>().enabled = b;
            currentChild.GetComponent<Seeker>().enabled = b;

            if (!b)
            {
                currentChild.position = currentChild.GetComponent<Seeker>().idlePos;
                Physics.SyncTransforms();
            }
        }
    }
}
