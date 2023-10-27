using System.Collections;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private GameObject player; // Changed to private as it will be assigned in Start method.

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // This line finds and assigns the player GameObject.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    private IEnumerator RespawnPlayer()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        player.transform.position = respawnPoint.position;

        player.SetActive(true);
    }
}