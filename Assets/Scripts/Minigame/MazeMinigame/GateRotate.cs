using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Player;
using UnityEditor;
using UnityEngine;

public class GateRotate : MonoBehaviour
{
    private bool rotateCooldown = false;
    public bool gateCollider = false;
    private Material gateColor;

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject;
        var playerScript = player.GetComponent<ItemManager>();
        // Check if tag is player and if the colour of player is the same as the fence
        if (player.CompareTag("Player") && !rotateCooldown && !gateCollider && playerScript.colours[playerScript.currentColour] == GetComponent<Renderer>().sharedMaterial)
        {
            Vector3 rotationAxis = Vector3.up;
            float rotationAngle = 90;

            transform.RotateAround(transform.position, rotationAxis, rotationAngle);
            Invoke(nameof(CooldownReset), 0.4f);
            rotateCooldown = true;
        }
    }

    

    private void CooldownReset()
    {
        rotateCooldown = false;
    }

}
