using System.Collections;
using Movement;
using Pickup.Player;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public static bool IsRespawning { get; private set; }

    [SerializeField] private Transform respawnPoint;
    
    private RotateHeadToMovement2 _rotateHeadToMovement2;

    private GameObject _player; // Changed to private as it will be assigned in Start method.
    private GameObject _hint;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // This line finds and assigns the player GameObject.
        _hint = _player.GetComponent<ItemManager>().hintText;
        GameObject head = GameObject.FindGameObjectWithTag("Head"); // Find the GameObject tagged "Head"
        _rotateHeadToMovement2 = head.GetComponent<RotateHeadToMovement2>(); // Get the RotateHeadToMovement2 component from the "Head" GameObject
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            StartCoroutine(RespawnPlayer());
            
        }
    }

    private IEnumerator RespawnPlayer()
    {
        IsRespawning = true;

        _player.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        _player.transform.position = respawnPoint.position;

        _hint.SetActive(false);
        _player.SetActive(true);
        IsRespawning = false; 
    }
}