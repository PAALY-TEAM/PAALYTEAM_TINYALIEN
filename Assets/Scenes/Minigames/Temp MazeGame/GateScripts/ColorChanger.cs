using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color changerColor;
    public GameObject player;

    private void Start()
    {
        GetComponent<Renderer>().material.color = changerColor;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Color Changed");
            player.GetComponent<PlayerColor>().playerColor = changerColor;
        }
    }

}
