using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRotate : MonoBehaviour
{
    public GameObject player;

    public float raycastDistance = 5.0f;
    bool rotateCooldown = false;
    public bool gateCollider = false;
    Material gateColor;

    void Start()
    {
        
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gateColor);
        Debug.Log(player.GetComponent<Renderer>().sharedMaterial);
        if (collision.gameObject.tag == "Player" && !rotateCooldown && !gateCollider && player.GetComponent<Renderer>().sharedMaterial == GetComponent<Renderer>().sharedMaterial)
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
