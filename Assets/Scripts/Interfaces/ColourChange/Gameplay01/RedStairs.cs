using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStairs : MonoBehaviour, IColourChange
{
    [SerializeField] GameObject StairCollider;
    private MeshCollider _collider;

    private void Awake()
    {
        _collider = StairCollider.GetComponent<MeshCollider>();
        _collider.enabled = false;
    }

    public void ColourChange()
    {
        //Enable Collider
        
        _collider.enabled = true;
    }
 
}

