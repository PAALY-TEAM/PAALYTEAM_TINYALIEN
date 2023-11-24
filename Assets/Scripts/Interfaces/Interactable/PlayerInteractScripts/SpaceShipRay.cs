using System.Collections;
using System.Collections.Generic;
using Pickup;
using Pickup.Player;
using UI.TextActivators;
using UnityEngine;

public class SpaceShipRay : MonoBehaviour, IPlayerInteract
{
    public void PlayerInteract()
    {
        //For future coding if we want visible display of crayons to have a origin position to the crayons
        var shipScript = GameObject.FindGameObjectWithTag("SpaceShip").GetComponent<ShipInventory>();
        var textScript = GameObject.FindGameObjectWithTag("SpaceShip").GetComponent<WhenCollectedCrayons>();
        var ims = GameObject.Find("MenuController").GetComponent<ItemManagerSaveLogic>();
        var itemManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>();
        //Adding the crayons to ship
        //Visible in the debug log
        for (int i = 0; i < ItemManager.NumbCarried.Length - 1; i++)
        {
            if (ItemManager.NumbCarried[i] > 0)
            {
                ItemManager.NumbStored[i] += ItemManager.NumbCarried[i];
                ItemManager.NumbCarried[i] = 0;
                if (ItemManager.NumbStored[i] > 0)
                {
                    itemManager.ChangeColourOfEnvironment(i+1);
                }
            }
        }
        shipScript.Display();
        textScript.CheckIfEnough();
        itemManager.ChangeAlienColour(0);
        itemManager.currentColour = 0;
        
        ims.SaveValues();
    }
}
