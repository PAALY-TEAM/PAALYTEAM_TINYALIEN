using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    public class CameraInput : MonoBehaviour
    {
        [Header("Character Input Value")]
        public Vector2 look;
        [Header("Camera Look Speed")]
        public float lookSpeedX = 1.0f;
        public float lookSpeedY = 1.0f;
        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private void Awake()
        {
            _playerInput = GetComponentInParent<PlayerInput>();
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }
        private void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }
        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}