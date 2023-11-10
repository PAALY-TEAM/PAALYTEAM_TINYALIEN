using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanRotate : MonoBehaviour
{
    public GameObject gateBody;
    


    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Gate"))
        {
            
            gateBody.GetComponent<GateRotate>().gateCollider = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            
            gateBody.GetComponent<GateRotate>().gateCollider = false;
        }
    }



}
