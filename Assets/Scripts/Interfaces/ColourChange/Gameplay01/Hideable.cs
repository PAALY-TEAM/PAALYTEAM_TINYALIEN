using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideable : MonoBehaviour, IColourChange
{
    [Header("Needs this number to be the same as the layer that obscures the vision of the guards")]
    [SerializeField] private int layerNr = 7;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }
    public void ColourChange()
    {
        gameObject.layer = layerNr;
        _collider.enabled = true;
    }
 
}

