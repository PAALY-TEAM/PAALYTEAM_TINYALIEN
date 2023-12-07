using System.Collections.Generic;
using Pickup;
using Pickup.Player;
using UnityEngine;
using UI;
using UnityEngine.SceneManagement;

                     
public class ItemManagerSaveLogic : MonoBehaviour
{
    [SerializeField] private TempDisableMovement  tempDisableMovement;
    private ItemManager _itemManager;
    
    private int[] savedShipStorage;
    private int[] savedPlayerStorage;
    private List<string> savedCrayonCounter;
    // No longer in use
    // private bool[] savedVisitedState;
    private int savedCurrentColour;
    private Vector3 savedPos;
    private int[] savedCrayonLost;

    private GameObject Player;
    private int CurrentScene;
    private CrayonCounter CrayonCounter;
    private CrayonLost _crayonLost;
                     
    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        _itemManager = Player.GetComponent<ItemManager>();
        CrayonCounter = GameObject.Find("CrayonCounter").GetComponent<CrayonCounter>();
        _crayonLost = GameObject.Find("CrayonCounter").GetComponent<CrayonLost>();
    }
                     
    //Set new value for current scene, run by ItemManager MySceneLoader();
    public void SaveValues()
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        savedShipStorage = new int[ItemManager.NumbStored.Length];
        savedPlayerStorage = new int[ItemManager.NumbCarried.Length];
        savedCrayonCounter = new List<string>();
        savedCrayonLost = new int[ItemManager.NumbStored.Length];
        
        foreach (var crayon in CrayonCounter.savedCrayon[CurrentScene])
        {
            savedCrayonCounter.Add(crayon);
        }
        //savedVisitedState = new bool[ItemManager.NumbStored.Length];
        for (int i = 0; i < ItemManager.NumbCarried.Length; i++)
        {
            savedPlayerStorage[i] = ItemManager.NumbCarried[i];
            if (i < ItemManager.NumbStored.Length)
            {
                savedShipStorage[i] = ItemManager.NumbStored[i];
                //savedVisitedState[i] = _itemManager._isSceneVisited[CurrentScene][i];
                // Has problems because array is created after this the first time, but the values doesn't matter at this point
                if (CrayonLost.crayonLost != null)
                    savedCrayonLost[i] = CrayonLost.crayonLost[CurrentScene][i];
                else
                    savedCrayonLost[i] = 0;
                
                
                
            }
        }

        
        savedCurrentColour = _itemManager.currentColour;
        savedPos = Player.transform.position;
        
    }
    
                     
    public void ReloadScene() 
    {
        // Sets the player inventory to what it was when entering scene
        for (int i = 0; i < ItemManager.NumbCarried.Length; i++)
        {
            ItemManager.NumbCarried[i] = savedPlayerStorage[i];
            if (i < ItemManager.NumbStored.Length)
            {
                ItemManager.NumbStored[i] = savedShipStorage[i];
                //_itemManager._isSceneVisited[CurrentScene][i] = savedVisitedState[i];
                
                CrayonLost.crayonLost[CurrentScene][i] = savedCrayonLost[i];
            }
        }
                     
        CrayonCounter.savedCrayon[CurrentScene].Clear();
        foreach (var crayon in savedCrayonCounter)
        {
            CrayonCounter.savedCrayon[CurrentScene].Add(crayon);
        }
        _itemManager.MoveAlien(savedPos);
        _itemManager.currentColour = savedCurrentColour;
        _itemManager.ChangeAlienColour(savedCurrentColour);
        if (SceneManager.GetActiveScene().buildIndex == _itemManager.gameScene)
        {
            SceneManager.LoadScene(_itemManager.copyScene);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
       
    }
}
