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
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject secondTarget;
        [SerializeField] private Vector3 initialScale = Vector3.one;
        [SerializeField] private Vector3 targetScale;
        [SerializeField] private float pressDuration = 0.26f;
        [SerializeField] private float releaseDuration = 1.51f;

        [FormerlySerializedAs("OnMaxScaleReached")] public UnityEvent onMaxScaleReached;
        [FormerlySerializedAs("OnNotAtMaxScale")] public UnityEvent onNotAtMaxScale;

        private InputAction _sprintAction;

        void Awake()
        {
            _sprintAction = new InputAction("Sprint", InputActionType.Button, "<Keyboard>/shift");
            _sprintAction.performed += OnPress;
            _sprintAction.canceled += OnRelease;
        }

        void Start()
        {
            transform.localScale = initialScale;
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
        private void OnPress(InputAction.CallbackContext context)
        {
            OnScale();
            ScaleDownSecondObject();
            //sprintFeedback?.PlayFeedbacks();
        }

        private void OnRelease(InputAction.CallbackContext context)
        {
            EaseBackDown();
            ScaleUpSecondObject();
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
