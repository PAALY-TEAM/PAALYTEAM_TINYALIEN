using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRotate : MonoBehaviour
{
    public GameObject player;
    public Color gateColor;
    public float raycastDistance = 5.0f;
    bool rotateCooldown = false;
    public bool gateCollider = false;

    void Start()
    {
        GetComponent<Renderer>().material.color = gateColor;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !rotateCooldown && !gateCollider && player.GetComponent<PlayerColor>().playerColor == gateColor)
        {
            Vector3 rotationAxis = Vector3.up;
            float rotationAngle = 90;

            transform.RotateAround(transform.position, rotationAxis, rotationAngle);
            Invoke("cooldownReset", 0.4f);
            rotateCooldown = true;

        }

    }




    void cooldownReset()
    {
        rotateCooldown = false;
    }

}
