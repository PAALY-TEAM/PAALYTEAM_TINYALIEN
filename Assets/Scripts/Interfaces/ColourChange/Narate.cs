using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narate : MonoBehaviour, IColourChange
{
    public void ColourChange()
    {
        transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
    }
}
