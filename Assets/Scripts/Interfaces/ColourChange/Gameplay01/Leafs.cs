using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Leafs : MonoBehaviour, IColourChange
{
    private bool isColoured = false;
    private Vector3 scaleChange;
    int dir= 1;
    

    public void ColourChange()
    {
        isColoured = true;
    }

    private void Update()
    {
        if (isColoured)
        {
            transform.localScale += new UnityEngine.Vector3(1, 1, 1) * (dir * Time.deltaTime);
            if (transform.localScale.y <  1)
            {
                dir = 1;
            }
            else if (transform.localScale.y > 2)
            {
                dir = -1;
            }
        }
    }
}

