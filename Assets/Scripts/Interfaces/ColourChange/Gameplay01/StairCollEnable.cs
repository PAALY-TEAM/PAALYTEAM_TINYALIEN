using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCollEnable : MonoBehaviour, IColourChange
{
    private MeshCollider _collider;
    private MeshCollider _childCollider;

    private void Awake()
    {
        _collider = GetComponent<MeshCollider>();
        _childCollider = transform.GetChild(0).GetComponent<MeshCollider>();
        _collider.enabled = false;
        _childCollider.enabled = false;
    }

    public void ColourChange()
    {
        //Enable Collider
        _collider.enabled = true;
        _childCollider.enabled = true;
    }
}
