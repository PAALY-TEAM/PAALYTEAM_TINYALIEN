using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator02 : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    // Before rendering each frame..
    void Update () 
    {
        // Rotate the game object that this script is attached to by 15 in the X axis,
        // 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
        // rather than per frame.
        transform.Rotate (new Vector3 (0, -speed, 0) * Time.deltaTime);
    }
}
