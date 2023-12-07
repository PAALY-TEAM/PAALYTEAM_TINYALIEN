using System.Collections;
using Movement;
using Pickup.Player;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public static bool IsRespawning { get; private set; }

    [SerializeField] private Transform respawnPoint;

    private RotateHeadToMovement2 _rotateHeadToMovement2;
    private ShiftKeyHandler _shiftKeyHandler;

    private GameObject _player;
    private GameObject _hint;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _hint = _player.GetComponent<ItemManager>().hintText;
        GameObject head = GameObject.FindGameObjectWithTag("Head");
        _rotateHeadToMovement2 = head.GetComponent<RotateHeadToMovement2>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            StartCoroutine(RespawnPlayer());
        }
    }
    public void FindNewPoint(GameObject newPoint)
    {
        respawnPoint = newPoint.transform;
    }
    private IEnumerator RespawnPlayer()
    {
        IsRespawning = true;

        _player.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        _player.GetComponent<ItemManager>().MoveAlien(respawnPoint.position);
        _rotateHeadToMovement2.ResetRotation();

        _hint.SetActive(false);
        _player.SetActive(true);

        GameObject body = GameObject.FindGameObjectWithTag("SprintBody");
        if (body != null)
        {
            ShiftKeyHandler shiftKeyHandler = body.GetComponent<ShiftKeyHandler>();
            if (shiftKeyHandler != null)
            {
                shiftKeyHandler.AssignBodies();
                shiftKeyHandler.ResetHandler();
            }
        }
        IsRespawning = false;
    }
}