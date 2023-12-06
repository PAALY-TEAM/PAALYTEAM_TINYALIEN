using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    void Update()
    {
        transform.Translate(0,-7*Time.deltaTime,0);
    }


}
