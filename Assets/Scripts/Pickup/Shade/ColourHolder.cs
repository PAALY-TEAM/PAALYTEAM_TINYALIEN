using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Player;
using UnityEngine;

public class ColourHolder : MonoBehaviour
{
    public enum Colour
    {
        Red,
        Yellow,
        Blue,
        Green,
        Orange,
        Purple,
        Brown
    }

    public enum Shade
    {
        Normal,
        Light,
        Dark,
        Bland
    }
    
    [Header("Shades should be in this order: Normal, Light, Dark, Bland")] 
    [SerializeField] private Material[] redShades;
    [SerializeField] private Material[] yellowShades;
    [SerializeField] private Material[] blueShades;
    [SerializeField] private Material[] greenShades;
    [SerializeField] private Material[] orangeShades;
    [SerializeField] private Material[] purpleShades;
    [SerializeField] private Material[] brownShades;

    //Grey for starting colour
    [HideInInspector] public Material[] greyShades;
    
    [HideInInspector] public Material[][] EveryColourWithShades;

    
    
    private void Awake()
    {
        //Set length equal to number of colours
        EveryColourWithShades = new Material[][]
        {
            redShades,
            yellowShades,
            blueShades,
            greenShades,
            orangeShades,
            purpleShades,
            brownShades
        };
    }
}
