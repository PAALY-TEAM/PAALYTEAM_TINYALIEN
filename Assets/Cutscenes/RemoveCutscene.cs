using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCutscene : MonoBehaviour
{
    [SerializeField] private int playTime;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Destroy(gameObject, playTime);
    }
}
