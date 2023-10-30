using UnityEngine;

namespace ScriptsToDelete
{
    public class AlienInputManager : MonoBehaviour
    {
        /*
        public PlayerMovementV03 playerMovement;
        [SerializeField] private Vector3 _forwardAxis;
        [SerializeField] private Vector3 _rightAxis;
       
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool climb;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;
         
        public UnityEvent<Vector3> OnMoveVectorInput = new UnityEvent<Vector3>();
        public UnityEvent<Vector2> OnMoveInput = new UnityEvent<Vector2>();
        public UnityEvent<Vector2> OnLookInput = new UnityEvent<Vector2>();
        public UnityEvent<bool> OnJumpInput = new UnityEvent<bool>();
        public UnityEvent<bool> OnClimbInput = new UnityEvent<bool>();
        public UnityEvent<bool> OnSprintInput = new UnityEvent<bool>();
        

        private void Start()
        {
            playerMovement = FindObjectOfType<PlayerMovementV03>();
        }
        
#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if(cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnClimb(InputValue value)
        {
            
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }
#endif
        
        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
            Vector3 inputVector = new Vector3(move.x, 0, move.y);
            inputVector = Vector3.ClampMagnitude(inputVector, 1f);
            // Assuming you have a reference to the new refactored movement script
            playerMovement.HandleMoveInput(inputVector);
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            playerMovement._desiredJump |= newJumpState;
        }

        public void ClimbInput(bool newClimbState)
        {
            playerMovement._desiresClimbing = newClimbState;
        }
        
        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
            OnSprintInput.Invoke(sprint);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        } */
    }
}