using System.Collections.Generic;
using TMPro;
using UI.StateMenu.Scripts.State;
using UnityEngine;

namespace UI.StateMenu.Scripts
{
    //The state machine, which keeps track of everything
    public class MenuController : MonoBehaviour
    {
        [SerializeField] 
        private ItemManagerSaveLogic itemManagerSaveLogic;
        
        //Drags = the different menus we have
        public _MenuState[] allMenus;

        private TempDisableMovement _tempDisableMovement;

        //The states we can choose from
        public enum MenuState
        {
            Game, Main, Settings, Help
        }
        //State-object dictionary to make it easier to activate a menu 
        private Dictionary<MenuState, _MenuState> menuDictionary = new Dictionary<MenuState, _MenuState>();

        //The current active menu
        private _MenuState activeState;

        //To easier jump back one step, we can use a stack
        //This was also suggested in the Game Programming Patterns book
        //If so we don't have to hard-code in each state what happens when we jump back one step
        private Stack<MenuState> stateHistory = new Stack<MenuState>();
        
        // To show player how many crayons are left to collect in scene
        [SerializeField] private TextMeshProUGUI showCrayonsLeft;
        
        void Start()
        {
            _tempDisableMovement = GetComponent<TempDisableMovement>();
            //Put all menus into a dictionary
            foreach (_MenuState menu in allMenus)
            {
                if (menu == null)
                {
                    continue;
                }

                //Inject a reference to this script into all menus
                menu.InitState(menuController: this);

                //Check if this key already exists, because it means we have forgotten to give a menu its unique key
                if (menuDictionary.ContainsKey(menu.state))
                {
                    Debug.LogWarning($"The key <b>{menu.state}</b> already exists in the menu dictionary!");

                    continue;
                }
            
                menuDictionary.Add(menu.state, menu);
            }

            //Deactivate all menus
            foreach (MenuState state in menuDictionary.Keys)
            {
                menuDictionary[state].gameObject.SetActive(false);
            }

            //Activate the default menu
            SetActiveState(MenuState.Game);
        }
        
        void Update()
        {
            //Jump back one menu step when we press escape
            if (Input.GetButtonDown("Pause"))
            {
                activeState.JumpBack();
                
            }
        }

        //Jump back one step = what happens when we press escape or one of the back buttons
        public void JumpBack()
        {
            //If we have just one item in the stack then, it means we are at the state we set at start, so we have to jump forward
            if (stateHistory.Count <= 1)
            {
                SetActiveState(MenuState.Main);
                // Display number of crayons left in scene
                showCrayonsLeft.text = "Crayons remaining: " + CrayonsCollected.NumbOfCrayonsInScene();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                _tempDisableMovement.OnPauseGame(true);
            }
            else
            {
                //Remove one from the stack
                stateHistory.Pop();

                //Activate the menu that's on the top of the stack
                SetActiveState(stateHistory.Peek(), isJumpingBack: true);
            }
            if (stateHistory.Count <= 1)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _tempDisableMovement.OnResumeGame();
            }
        }

        //Activate a menu
        public void SetActiveState(MenuState newState, bool isJumpingBack = false)
        {
            //First check if this menu exists
            if (!menuDictionary.ContainsKey(newState))
            {
                Debug.LogWarning($"The key <b>{newState}</b> doesn't exist so you can't activate the menu!");

                return;
            }

            //Deactivate the old state
            if (activeState != null)
            {
                activeState.gameObject.SetActive(false);
            }

            //Activate the new state
            activeState = menuDictionary[newState];

            activeState.gameObject.SetActive(true);

            //If we are jumping back we shouldn't add to history because then we will get doubles
            if (!isJumpingBack)
            {
                stateHistory.Push(newState);
            }
        }
        
        //Quit game
        public void QuitGame()
        {
            #if UNITY_EDITOR
            Debug.Log("Quit Scene");
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
