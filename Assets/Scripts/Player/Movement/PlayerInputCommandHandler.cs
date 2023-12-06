using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using Movement.Commands;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerInputCommandHandler : MonoBehaviour
    {
        public static PlayerInputCommandHandler Instance { get; private set; }

        private PlayerInput _playerInput;
        private PlayerMovementV03 _playerMovement;
        private ShiftKeyHandler _shiftKeyHandler;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Dictionary<InputAction, Command> _commandActionMap = new Dictionary<InputAction, Command>();

        // Cache the InputActions and their values
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;
        private InputAction _sprintAction;
        private Vector2 _moveDirection;

        public InputActionReference actionReference;
        private InputActionRebindingExtensions.RebindingOperation _rebindOperation;
        private bool _isRebindProcessStarted = false;


        public void Initialize(InputActionReference actionReference)
        {
            this.actionReference = actionReference;
            _rebindOperation = actionReference.action.PerformInteractiveRebinding();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            _playerInput = GetComponent<PlayerInput>();
            _playerMovement = GetComponent<PlayerMovementV03>();
            _shiftKeyHandler = GetComponent<ShiftKeyHandler>();
            
            // Cache the InputActions
            _moveAction = _playerInput.actions.FindAction("Move");
            _jumpAction = _playerInput.actions.FindAction("Jump");
            _interactAction = _playerInput.actions.FindAction("Interact");
            _sprintAction = _playerInput.actions.FindAction("Sprint");
            
            _commandActionMap = new Dictionary<InputAction, Command>
            {
                // Initialize the actionCommandMap dictionary
                [_playerInput.actions.FindAction("Move")] = new MoveCommand(_playerMovement, Vector2.zero),
                [_playerInput.actions.FindAction("Jump")] = new JumpCommand(_playerMovement),
                [_playerInput.actions.FindAction("Climb")] = new ClimbCommand(_playerMovement),
                [_playerInput.actions.FindAction("Interact")] = new InteractCommand(_playerMovement),
                [_playerInput.actions.FindAction("Sprint")] = new SprintCommand(_shiftKeyHandler)
            };

            // Enable all InputActions
            foreach (InputAction action in _commandActionMap.Keys)
            {
                action.Enable();
            }
        }
        private void Update()
        {
            
            // Use the cached InputActions and their values
            _moveDirection = _moveAction.ReadValue<Vector2>();

            // Execute the appropriate command when the corresponding key is pressed
            foreach (KeyValuePair<InputAction, Command> actionCommandPair in _commandActionMap)
            {
                if (!actionCommandPair.Key.triggered) continue;
                if (actionCommandPair.Value is MoveCommand moveCommand)
                {
                    // Update the direction in the MoveCommand
                    moveCommand.UpdateDirection(new Vector3(_moveDirection.x, 0f, _moveDirection.y));
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
        private void OnDestroy()
        {
            _rebindOperation?.Dispose();
        }
        public void SwapCommands(InputAction actionToRebind, string newBinding)
        {
            // Check if the actionToRebind is in the _commandActionMap dictionary
            if (!_commandActionMap.ContainsKey(actionToRebind))
                throw new KeyNotFoundException("The action to rebind is not found in the command action map.");

            // Check if the newBinding is a valid input binding string
            // This is a basic check and may not cover all edge cases
            if (string.IsNullOrEmpty(newBinding) || !newBinding.Contains("/"))
                throw new ArgumentException("The new binding is not a valid input binding string.");

            // Get the index of the binding in the action's bindings
            int bindingIndex = actionToRebind.bindings.IndexOf(b => b.action == actionToRebind.name);

            // Perform the rebinding
            actionToRebind.PerformInteractiveRebinding(bindingIndex)
                .WithRebindAddingNewBinding(newBinding)
                .Start();
        }
        public void StartRebindProcess(InputActionReference actionReference, RebindInputUI rebindInputUI)
        {
            if (!_isRebindProcessStarted)
            {
                _isRebindProcessStarted = true;
                rebindInputUI.StartRebindProcess();
            }
        }

        public void OnRebindComplete(InputActionReference actionReference)
        {
            // Get the InputAction from the InputActionReference
            InputAction action = actionReference.action;

            // Get the Command from the InputAction
            Command command = GetCommand(action);

            // Remove the old InputAction from the dictionary
            _commandActionMap.Remove(action);

            // Add the new InputAction to the dictionary
            _commandActionMap[action] = command;
            
            _isRebindProcessStarted = false;
        }
        public Command GetCommand(InputAction action)
        {
            _commandActionMap.TryGetValue(action, out Command command);
            return command;
        }
        public InputAction GetAction(string actionName)
        {
            return _playerInput.actions.FindAction(actionName);
        }
        
        public Vector2 GetMoveInput()
        {
            InputAction moveAction = _playerInput.actions.FindAction("Move");
            Vector2 direction = moveAction.ReadValue<Vector2>();
            ((MoveCommand)_commandActionMap[moveAction]).Execute();
            return direction;
        }
        
        public bool GetJumpInput()
        {
            if (_playerInput.actions.FindAction("Jump").ReadValue<float>() > 0.5f)
            {
                ((JumpCommand)_commandActionMap[_playerInput.actions.FindAction("Jump")]).Execute();
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
            if (_playerInput.actions.FindAction("Interact").WasPressedThisFrame())
            {
                ((InteractCommand)_commandActionMap[_playerInput.actions.FindAction("Interact")]).Execute();
                return true;
            }
            return false;
        }

        public bool GetSprintInput()
        {
            if (_playerInput.actions.FindAction("Sprint").ReadValue<float>() > 0.5f)
            {
                ((SprintCommand)_commandActionMap[_playerInput.actions.FindAction("Sprint")]).Execute();
                return true;
            }
            return false;
        }
    }
}
