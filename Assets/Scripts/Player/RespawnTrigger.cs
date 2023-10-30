using System.Collections;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private GameObject _player; // Changed to private as it will be assigned in Start method.

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); // This line finds and assigns the player GameObject.
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
        _player.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        _player.transform.position = respawnPoint.position;

        _player.SetActive(true);
    }
}