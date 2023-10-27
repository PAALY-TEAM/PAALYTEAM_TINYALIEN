using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour, IColourChange
{
    private GameObject dirLight;
    private Light myLight;

    void Awake()
    {
        dirLight = GameObject.Find("Directional Light");
        myLight = dirLight.GetComponent<Light>();
    }

    public void ColourChange()
    {
        myLight.intensity = 1;
    }
}
