using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using ScriptsToDelete;
using UnityEngine;
//https://www.youtube.com/watch?v=CQEqJ4TJzUk Scene transition tutorial
public class TriggerInteractionBase : MonoBehaviour, IInteractable
{
    public GameObject Player { get; set; }
    public bool CanInteract { get; set; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (CanInteract)
        {
            if (PlayerMovementV02.WasInteractPressed)
            {
                Interact();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        CanInteract = false;
    }

    public virtual void Interact() {}
}
