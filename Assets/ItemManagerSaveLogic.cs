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
    
    public int[] savedShipStorage;
    public int[] savedPlayerStorage;
    public List<string> savedCrayonCounter;
    public bool[] savedVisitedState;
    public int savedCurrentColour;
    public Vector3 savedPos;
                     
    public GameObject Player { get; private set; }
    public int CurrentScene { get; private set; }
    public CrayonCounter CrayonCounter { get; private set; }
                     
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        _itemManager = Player.GetComponent<ItemManager>();
        CrayonCounter = GameObject.Find("CrayonCounter").GetComponent<CrayonCounter>();
    }
                     
    //Set new value for current scene, run by ItemManager MySceneLoader();
    public void SaveValues()
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        savedShipStorage = new int[ItemManager.NumbStored.Length];
        savedPlayerStorage = new int[ItemManager.NumbCarried.Length];
        savedCrayonCounter = new List<string>();
        foreach (var crayon in CrayonCounter.savedCrayon[CurrentScene])
        {
            savedCrayonCounter.Add(crayon);
        }
        savedVisitedState = new bool[ItemManager.NumbStored.Length];
        for (int i = 0; i < ItemManager.NumbCarried.Length; i++)
        {
            savedPlayerStorage[i] = ItemManager.NumbCarried[i];
            if (i < ItemManager.NumbStored.Length)
            {
                savedShipStorage[i] = ItemManager.NumbStored[i];
                savedVisitedState[i] = _itemManager._isSceneVisited[CurrentScene][i];
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
                _itemManager._isSceneVisited[CurrentScene][i] = savedVisitedState[i];
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
