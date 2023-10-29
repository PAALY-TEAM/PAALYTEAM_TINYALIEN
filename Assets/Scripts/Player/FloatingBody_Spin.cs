using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBodySpin : MonoBehaviour
{
    [SerializeField]private float speed = 40f;
    void Update()
    {
            transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
    }
}
