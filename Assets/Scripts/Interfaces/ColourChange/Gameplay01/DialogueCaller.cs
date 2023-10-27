using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueCaller : MonoBehaviour, IColourChange
{
    private bool isColoured = false;
    public void ColourChange()
    {
        if (!isColoured)
            transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();

        isColoured = true;
    }
}
