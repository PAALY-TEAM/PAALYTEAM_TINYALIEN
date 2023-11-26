using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRotate : MonoBehaviour
{

    public float raycastDistance = 5.0f;
    bool rotateCooldown = false;
    public bool gateCollider = false;
    Material gateColor;
    


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !rotateCooldown && !gateCollider)
        {
            var material1 = collision.transform.Find("AlienBodyBeforeDeform").GetComponent<Renderer>().sharedMaterial;
            var material2 = collision.transform.Find("AlienBody_Floating").GetComponent<Renderer>().sharedMaterial;
            if (material1 == GetComponent<Renderer>().sharedMaterial || material2 == GetComponent<Renderer>().sharedMaterial)
            {
                Vector3 rotationAxis = Vector3.up;
                float rotationAngle = 90;

                transform.RotateAround(transform.position, rotationAxis, rotationAngle);
                Invoke("cooldownReset", 0.4f);
                rotateCooldown = true;
            }
        }
    }

    void cooldownReset()
    {
        rotateCooldown = false;
    }

}
