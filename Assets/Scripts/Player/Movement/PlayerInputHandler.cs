using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _climbAction;
        private InputAction _interactAction;
        private InputAction _sprintAction;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions.FindAction("Move");
            _jumpAction = _playerInput.actions.FindAction("Jump");
            _climbAction = _playerInput.actions.FindAction("Climb");
            _interactAction = _playerInput.actions.FindAction("Interact");
            _sprintAction = _playerInput.actions.FindAction("Sprint");
        }
        #region Input Handling

        private void OnEnable() {
            // Enable the Input Actions
            _moveAction.Enable();
            _jumpAction.Enable();
            _climbAction.Enable();
            _interactAction.Enable();
            _sprintAction.Enable();
        }
        private void OnDisable() {
            // Disable the Input Actions
            _moveAction.Disable();
            _jumpAction.Disable();
            _climbAction.Disable();
            _interactAction.Disable();
            _sprintAction.Disable();
        }
        private void OnDestroy()
        {           
            // Disable the Input Actions
            _moveAction.Disable();
            _jumpAction.Disable();
            _climbAction.Disable();
            _interactAction.Disable();
            _sprintAction.Disable();
        }
        #endregion
        public Vector2 GetMoveInput()
        {
            return _moveAction.ReadValue<Vector2>();
        }

        public bool GetJumpInput()
        {
            return _jumpAction.triggered;
        }

        public bool GetClimbInput()
        {
            return _climbAction.ReadValue<float>() > 0.5f;
        }

        public bool GetInteractInput()
        {
            return _interactAction.WasPressedThisFrame();
        }

        public bool GetSprintInput()
        {
            return _sprintAction.ReadValue<float>() > 0.5f;
        }
    }
}
