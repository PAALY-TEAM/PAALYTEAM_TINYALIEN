using System.Collections.Generic;
using Movement.Commands;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerInput playerInput;
        private PlayerMovementV03 _playerMovement;
        private ShiftKeyHandler _shiftKeyHandler;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Dictionary<InputAction, Command> _commandActionMap = new Dictionary<InputAction, Command>();

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            _playerMovement = GetComponent<PlayerMovementV03>();
            _shiftKeyHandler = GetComponent<ShiftKeyHandler>();
            
            _commandActionMap = new Dictionary<InputAction, Command>
            {
                // Initialize the actionCommandMap dictionary
                [playerInput.actions.FindAction("Move")] = new MoveCommand(_playerMovement, Vector2.zero),
                [playerInput.actions.FindAction("Jump")] = new JumpCommand(_playerMovement),
                [playerInput.actions.FindAction("Climb")] = new ClimbCommand(_playerMovement),
                [playerInput.actions.FindAction("Interact")] = new InteractCommand(_playerMovement),
                [playerInput.actions.FindAction("Sprint")] = new SprintCommand(_shiftKeyHandler)
            };

            // Enable all InputActions
            foreach (InputAction action in _commandActionMap.Keys)
            {
                action.Enable();
            }
        }
        private void Update()
        {
            
            // Get the player's input
            Vector2 direction = playerInput.actions.FindAction("Move").ReadValue<Vector2>();

            // Execute the appropriate command when the corresponding key is pressed
            foreach (KeyValuePair<InputAction, Command> actionCommandPair in _commandActionMap)
            {
                if (!actionCommandPair.Key.triggered) continue;
                if (actionCommandPair.Value is MoveCommand moveCommand)
                {
                    // Update the direction in the MoveCommand
                    moveCommand.UpdateDirection(new Vector3(direction.x, 0f, direction.y));
                    continue;
                }
                // Execute the command
                actionCommandPair.Value.Execute();
            }
        }
        private void OnDisable()
        {
            // Disable all InputActions
            foreach (InputAction action in _commandActionMap.Keys)
            {
                action.Disable();
            }
        }
        public void SwapCommands(InputAction actionToRebind, string newBinding)
        {
            // Check if the actionToRebind is in the _commandActionMap dictionary
            if (!_commandActionMap.ContainsKey(actionToRebind))
            {
                Debug.LogError("The action to be rebound is not in the _commandActionMap dictionary.");
                return;
            }

            // Check if the newBinding is a valid input binding string
            // This is a basic check and may not cover all edge cases
            if (string.IsNullOrEmpty(newBinding) || !newBinding.Contains("/"))
            {
                Debug.LogError("The new key binding is not a valid input binding string.");
                return;
            }

            // Create a new InputAction with the new binding
            InputAction newAction = new InputAction(binding: newBinding);

            // Get the command that should be executed when the action is performed
            Command command = _commandActionMap[actionToRebind];

            // Add the command execution to the performed event of the new InputAction
            newAction.performed += callbackContext => command.Execute();

            newAction.Enable();

            // Replace the old action in the dictionary
            _commandActionMap[newAction] = command;

            // Remove the old action from the dictionary
            _commandActionMap.Remove(actionToRebind);
            
            // Disable the old action
            actionToRebind.Disable();
        }
        public Command GetCommand(InputAction action)
        {
            _commandActionMap.TryGetValue(action, out Command command);
            return command;
        }
        public InputAction GetAction(string actionName)
        {
            return playerInput.actions.FindAction(actionName);
        }
        
        public Vector2 GetMoveInput()
        {
            InputAction moveAction = playerInput.actions.FindAction("Move");
            Vector2 direction = moveAction.ReadValue<Vector2>();
            ((MoveCommand)_commandActionMap[moveAction]).Execute();
            return direction;
        }


        public bool GetJumpInput()
        {
            if (playerInput.actions.FindAction("Jump").ReadValue<float>() > 0.5f)
            {
                ((JumpCommand)_commandActionMap[playerInput.actions.FindAction("Jump")]).Execute();
                return true;
            }
            return false;
        }

        public bool GetClimbInput()
        {
            /*
            if (_climbAction.ReadValue<float>() > 0.5f)
            {
                ((ClimbCommand)_commandActionMap[playerInput.actions.FindAction("Climb")]).Execute();
                return true;
            }*/
            return false;
        }

        public bool GetInteractInput()
        {
            if (playerInput.actions.FindAction("Interact").WasPressedThisFrame())
            {
                ((InteractCommand)_commandActionMap[playerInput.actions.FindAction("Interact")]).Execute();
                return true;
            }
            return false;
        }

        public bool GetSprintInput()
        {
            if (playerInput.actions.FindAction("Sprint").ReadValue<float>() > 0.5f)
            {
                ((SprintCommand)_commandActionMap[playerInput.actions.FindAction("Sprint")]).Execute();
                return true;
            }
            return false;
        }
    }
}
