using DG.Tweening;
using MoreMountains.Feedbacks;
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
        [SerializeField] private GameObject mainBodyTarget;
        [SerializeField] private GameObject secondTarget;
        [SerializeField] private Vector3 initialScale = Vector3.one;
        [SerializeField] private Vector3 targetScale;
        [SerializeField] private float pressDuration = 0.26f;
        [SerializeField] private float releaseDuration = 1.51f;

        [FormerlySerializedAs("OnMaxScaleReached")] public UnityEvent onMaxScaleReached;
        [FormerlySerializedAs("OnNotAtMaxScale")] public UnityEvent onNotAtMaxScale;
        
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

        void Start()
        {
            transform.localScale = initialScale;
            // Call InitialSetup at the start of the game
            InitialSetup();
        }
        
        private void InitialSetup()
        {
            OnScale();
            ScaleDownSecondObject();
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
                onMaxScaleReached.Invoke();
                sprintFeedback?.PlayFeedbacks();

            }
            else
            {
                onNotAtMaxScale.Invoke();
                sprintFeedback?.StopFeedbacks();
            }
        }
        public void OnPress(InputAction.CallbackContext context)
        {
            EaseBackDown();
            ScaleUpSecondObject();
            _playerMovement.SetCurrentSpeed(_playerMovement.GetMaxRollingSpeed());
        }

        public void OnRelease(InputAction.CallbackContext context)
        {
            // Check if the ShiftKeyHandler object still exists
            if (this != null)
            {
                OnScale();
                ScaleDownSecondObject();
                _playerMovement.SetCurrentSpeed(_playerMovement.GetMaxFloatingSpeed());
            }
        }

        private void OnScale()
        {
            transform.DOKill(); // Stop any ongoing tween
            transform.DOScale(targetScale, pressDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => onMaxScaleReached.Invoke());
            
        }

        private void ScaleDownSecondObject()
        {
            secondTarget.transform.DOKill(); // Stop any ongoing tween
            secondTarget.transform.DOScale(Vector3.zero, pressDuration)
                .SetEase(Ease.InOutSine);
        }

        private void ScaleUpSecondObject()
        {
            secondTarget.transform.DOKill(); // Stop any ongoing tween
            secondTarget.transform.DOScale(Vector3.one, releaseDuration)
                .SetEase(Ease.OutBounce);
        }

        private void EaseBackDown()
        {
            if (this == null)
            {
                // The ShiftKeyHandler object has been destroyed, so return immediately
                return;
            }
            
            transform.DOKill(); // Stop any ongoing tween
            transform.DOScale(initialScale, releaseDuration)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => onNotAtMaxScale.Invoke());
        }

        private void OnEnable()
        {
            _sprintAction.Enable();
        }

        private void OnDisable()
        {
            _sprintAction.Disable();
        }
    
    }
}
