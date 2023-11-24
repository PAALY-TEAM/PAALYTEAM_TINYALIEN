using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [Header("The transform of where all the crayons should spawn")]
    public Transform[] crayonLocation;
    [Header("Transform of where the player should be sent when captured")]
    public Transform playerLocation;

}
