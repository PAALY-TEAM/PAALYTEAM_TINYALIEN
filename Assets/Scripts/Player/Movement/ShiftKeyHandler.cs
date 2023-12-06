using System;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Movement
{
    public class ShiftKeyHandler : MonoBehaviour
    {
        [Header("Feedbacks")]
        [SerializeField] private MMFeedbacks sprintFeedback;
        [SerializeField] private GameObject sprintingBody;
        [SerializeField] private GameObject walkingBody;
        [SerializeField] private Vector3 initialScale = Vector3.one;
        [SerializeField] private Vector3 targetScale;
        [SerializeField] private float pressDuration = 0.26f;
        [SerializeField] private float releaseDuration = 1.51f;

        // Define an enum to represent the state of the player
        public enum PlayerState
        {
            Walking,
            Sprinting
        }
        // Add a field to store the current state
        [SerializeField] private PlayerState currentState = PlayerState.Walking;
        [SerializeField] private bool isSprintingBodyActiveFirst = false;
        /*
        [FormerlySerializedAs("OnMaxScaleReached")] public UnityEvent onMaxScaleReached;
        [FormerlySerializedAs("OnNotAtMaxScale")] public UnityEvent onNotAtMaxScale;
        */
        private PlayerMovementV03 _playerMovement;

        private PlayerInput _playerInput;
        private InputAction _sprintAction;

        void Awake()
        {
            // Get the PlayerInput component from the player object
            _playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

            // Find the _sprintAction from the PlayerInput.actions list
            _sprintAction = _playerInput.actions.FindAction("Sprint");

            // Get the PlayerMovementV03 component from the player object
            _playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovementV03>();
            
            _sprintAction.performed += OnPress;
            _sprintAction.canceled += OnRelease;
        }
        #region InputSystem On-Enable/-Disable
        private void OnEnable()
        {
            _sprintAction.Enable();
        }

        private void OnDisable()
        {
            _sprintAction.performed -= OnPress;
            _sprintAction.canceled -= OnRelease;
            _sprintAction.Disable();
        }
        #endregion
        void Start()
        {
            transform.localScale = initialScale;
            // Call InitialSetup at the start of the game
            InitialSetup();
        }
        
        private void InitialSetup()
        {
            // Set the initial state based on the currentState variable
            switch (currentState)
            {
                case PlayerState.Walking:
                    ScaleUpWalkingBody();
                    if (isSprintingBodyActiveFirst)
                    {
                        ScaleDownSprintingBody();
                    }
                    break;
                case PlayerState.Sprinting:
                    ScaleUpSprintingBody();
                    if (isSprintingBodyActiveFirst)
                    {
                        ScaleDownWalkingBody();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _playerMovement.SetCurrentSpeed(_playerMovement.GetMaxRollingSpeed());
        }
        
        void Update()
        {
            CheckScaleStatus();
        }

        private void CheckScaleStatus()
        {
            if (transform.localScale == targetScale)
            {
                //onMaxScaleReached?.Invoke();
                sprintFeedback?.PlayFeedbacks();

            }
            else
            {
                //onNotAtMaxScale?.Invoke();
                sprintFeedback?.StopFeedbacks();
            }
        }
        public void OnPress(InputAction.CallbackContext context)
        {
            // Update the state to Sprinting when the sprint key is pressed
            currentState = PlayerState.Sprinting;

            ScaleUpSprintingBody();
            ScaleDownWalkingBody();
            _playerMovement.SetCurrentSpeed(_playerMovement.GetMaxRollingSpeed());
        }

        public void OnRelease(InputAction.CallbackContext context)
        {
            // Update the state to Walking when the sprint key is released
            currentState = PlayerState.Walking;

            EaseBackDown();
            ScaleUpWalkingBody();
            _playerMovement.SetCurrentSpeed(_playerMovement.GetMaxFloatingSpeed());
        }

        private void OnScale()
        {
            transform.DOKill(); // Stop any ongoing tween
            transform.DOScale(targetScale, pressDuration)
                .SetEase(Ease.InOutSine);
            //.OnComplete(() => onMaxScaleReached.Invoke());

        }
        private void ScaleUpWalkingBody()
        {
            if (walkingBody == null)
                return;
            walkingBody.transform.DOKill();                // Stop any ongoing tween
            walkingBody.transform.DOScale(Vector3.one, 1f) // Duration set to 1 second
                .SetEase(Ease.OutBounce);                  // Bounce effect
        }
        
        private void ScaleDownWalkingBody()
        {
            if (walkingBody == null)
                return;
            walkingBody.transform.DOKill(); // Stop any ongoing tween
            walkingBody.transform.DOScale(Vector3.zero, pressDuration)
                .SetEase(Ease.InOutSine);
        }

        private void ScaleUpSprintingBody()
        {
            if (sprintingBody == null)
                return;
            sprintingBody.transform.DOKill(); // Stop any ongoing tween
            sprintingBody.transform.DOScale(Vector3.one, pressDuration)
                .SetEase(Ease.OutBounce);
        }
        
        private void ScaleDownSprintingBody()
        {
            if (sprintingBody == null)
                return;
            sprintingBody.transform.DOKill(); // Stop any ongoing tween
            sprintingBody.transform.DOScale(Vector3.zero, pressDuration)
                .SetEase(Ease.InOutSine);
        }

        private void EaseBackDown()
        {
            if (sprintingBody == null)
            {
                // The sprintingBody object has been destroyed, so return immediately
                return;
            }

            sprintingBody.transform.DOKill(); // Stop any ongoing tween
            sprintingBody.transform.DOScale(initialScale, releaseDuration)
                .SetEase(Ease.OutBounce);
        }
    }
}
