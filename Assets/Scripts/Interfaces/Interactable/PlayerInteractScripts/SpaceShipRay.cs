using System.Collections;
using System.Collections.Generic;
using Pickup;
using Pickup.Player;
using UnityEngine;

public class SpaceShipRay : MonoBehaviour, IPlayerInteract
{
    private ItemManagerSaveLogic IMS;
    public void PlayerInteract()
    {
        //For future coding if we want visible display of crayons to have a origin position to the crayons
        var shipScript = transform.parent.GetComponent<ShipInventory>();
        //Adding the crayons to ship
        //Visible in the debug log
        for (int i = 0; i < colours.Length - 1; i++)
        {
            if (ItemManager.NumbCarried[i] > 0)
            {
                ItemManager.NumbStored[i] += ItemManager.NumbCarried[i];
                ItemManager.NumbCarried[i] = 0;
                if (ItemManager.NumbStored[i] > 0)
                {
                    ChangeColourOfEnvironment(i+1);
                }
            }
        }
        shipScript.Display();
        ChangeAlienColour(0);
        currentColour = 0;
        if (_pauseMenu)
        {
            _pauseMenu.SaveValues();
        }

        if (_IMSLogic)
        {
            _IMSLogic.SaveValues();
        }
    }
}
