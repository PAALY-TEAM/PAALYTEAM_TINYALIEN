using Interfaces;
using MoreMountains.Tools;
using Movement;
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
            if (PlayerMovementV03.WasInteractPressed)
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
        if (collision.gameObject == Player)
        {
            CanInteract = false;
        }
    }

    public virtual void Interact() {}
}
