using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Pickup;
using Pickup.Player;
using UnityEngine;
using UnityEngine.Serialization;

public class ProgressBarLogic : MonoBehaviour
{
    private static ItemManager  itemManager;
    private static bool isSaved;

    [SerializeField]
    private float _currentValue;  // Field to actually hold the value
    private static float maxValue;

    
    // a test button to display in our inspector and let us call the ChangeBarValue method
    [MMInspectorButton("ChangeBarValue")] public bool ChangeBarValueBtn;
    //
    [Header("Assign MMFeedback progress bar")] 
    [SerializeField] private MMProgressBar progressBar;
    [SerializeField] private MMProgressBar progressCircle;
    //
    private GameObject spaceShip;
    private ShipInventory shipInventory;

    
    void ChangeBarValue()
    {
        progressBar.UpdateBar(CurrentValue, 0f, MaxValue);
        progressCircle.UpdateBar(CurrentValue, 0f, MaxValue);
    }
    
    private void Awake()
    {
        if (!isSaved)
        {
            spaceShip = GameObject.Find("SpaceShip");
            shipInventory = spaceShip.GetComponent<ShipInventory>();
            itemManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!isSaved)
        {
            MaxValue = shipInventory.maxCrayonOnShip;
            isSaved = true;
        }
        
        itemManager.CrayonPickedUp += OnCrayonPickedUp;
    }
    private void OnDestroy()
    {
        itemManager.CrayonPickedUp -= OnCrayonPickedUp;
    }
    public void OnCrayonPickedUp()
    {
        CurrentValue = itemManager.CrayonProgress;
        // Ensures that the UI gets updated as soon as a crayon gets picked up
        ChangeBarValue(); 
    }

    #region Logic for seting the max value as the same as number of crayons allowed on the ship
    public float MaxValue
    {
        get { return maxValue; }
        set { maxValue = value; _currentValue = Mathf.Clamp(_currentValue, 0, maxValue); }
    }
    //making sure that the number of picked up crayons dont exceed the max
    public float CurrentValue 
    {
        get { return _currentValue; }
        set
        {
            _currentValue = Mathf.Clamp(value, 0, maxValue);
        }
    }
    #endregion

}
