using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject playerColor;
    [SerializeField] private Material targetMaterial;
    [SerializeField] private GameObject hintText;
    [Header("Destination to teleport")]
    [SerializeField] private GameObject currentScene;
    [SerializeField] private GameObject nextScene;
    private bool isClose;

    private Renderer rend;
    private Renderer playerRenderer;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        playerColor = GameObject.FindGameObjectWithTag("PlayerColor");
        playerRenderer = playerColor.GetComponent<Renderer>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("PlayerColor"))
        {
            if (playerRenderer.sharedMaterial == targetMaterial)
            {
                hintText.SetActive(true);
                isClose = true;
            }
            else
            {
                hintText.SetActive(false);
                isClose = false;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("PlayerColor"))
        {
            hintText.SetActive(false);
            isClose = false;
        }
    }

    private void Update()
    {
        if (isClose && Input.GetButtonDown("Interact"))
        {
            nextScene.SetActive(true);
            currentScene.SetActive(false);
            hintText.SetActive(false);
            isClose = false;
        }
    }
}