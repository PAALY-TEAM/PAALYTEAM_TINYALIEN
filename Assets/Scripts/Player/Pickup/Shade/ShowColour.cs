using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowColour : MonoBehaviour
{
    [SerializeField] private Sprite[] colours;

    private Image thisImage;
    private void Awake()
    {
        thisImage = GetComponent<Image>();
        thisImage.sprite = null;
    }

    public void ChangeIcon(int colourIndex)
    {
        if (colourIndex == 0)
        {
            thisImage.sprite = null;
            return;
        }
        thisImage.sprite = colours[colourIndex-1];
    }
}
