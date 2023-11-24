using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBodySpin : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    private Quaternion _targetRotation;

    
    private void Start()
    {
        _targetRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        // Calculate the target rotation
        _targetRotation *= Quaternion.Euler(0, speed * Time.deltaTime, 0);

        // Smoothly rotate towards the target rotation
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation.eulerAngles.y, ref speed, Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
