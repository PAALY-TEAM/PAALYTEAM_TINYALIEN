using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RemoveOldInputSystem : MonoBehaviour
{
    private  StandaloneInputModule _standaloneInputModule;
    void Start()
    {
        _standaloneInputModule = GetComponent<StandaloneInputModule>();
        _standaloneInputModule.enabled = false;
    }

   
}
