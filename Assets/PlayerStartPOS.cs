using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPos : MonoBehaviour
{
   
    void Start()
    {
        GameObject.FindWithTag("Player").transform.position = transform.position;
        Physics.SyncTransforms();
    }

}
