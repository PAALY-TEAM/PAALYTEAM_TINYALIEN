using MoreMountains.Tools;
using Pickup;
using Pickup.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class ProgressBarLogic : MonoBehaviour
    {
        private static ItemManager  _itemManager;
        private static bool _isSaved;

        [FormerlySerializedAs("_currentValue")] [SerializeField]
        private float currentValue;  // Field to actually hold the value
        private static float _maxValue;

    
        // a test button to display in our inspector and let us call the ChangeBarValue method
        [FormerlySerializedAs("ChangeBarValueBtn")] [MMInspectorButton("ChangeBarValue")] public bool changeBarValueBtn;
        //
        [Header("Assign MMFeedback progress bar")] 
        [SerializeField] private MMProgressBar progressBar;
        [SerializeField] private MMProgressBar progressCircle;
        //
        private GameObject _spaceShip;
        private ShipInventory _shipInventory;

    
        void ChangeBarValue()
        {
            progressBar.UpdateBar(CurrentValue, 0f, MaxValue);
            //progressCircle.UpdateBar(CurrentValue, 0f, MaxValue);
        }
    
        private void Awake()
        {
            if (!_isSaved)
            {
                _spaceShip = GameObject.Find("SpaceShip");
                _shipInventory = _spaceShip.GetComponent<ShipInventory>();
                _itemManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>();
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            if (!_isSaved)
            {
                MaxValue = _shipInventory.maxCrayonOnShip;
                _isSaved = true;
            }
        
            _itemManager.CrayonPickedUp += OnCrayonPickedUp;
        }
        private void OnDestroy()
        {
            _itemManager.CrayonPickedUp -= OnCrayonPickedUp;
        }
        public void OnCrayonPickedUp()
        {
            print("Crayon PickedUp");
            CurrentValue = _itemManager.CrayonProgress;
            // Ensures that the UI gets updated as soon as a crayon gets picked up
            ChangeBarValue(); 
        }

        #region Logic for seting the max value as the same as number of crayons allowed on the ship
        public float MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; currentValue = Mathf.Clamp(currentValue, 0, _maxValue); }
        }
        //making sure that the number of picked up crayons dont exceed the max
        public float CurrentValue 
        {
            get { return currentValue; }
            set
            {
                currentValue = Mathf.Clamp(value, 0, _maxValue);
            }
        }
        #endregion

    }
}
