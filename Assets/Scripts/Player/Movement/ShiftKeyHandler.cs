using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEditor;
using UnityEngine.Events;

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

    public UnityEvent OnMaxScaleReached;
    public UnityEvent OnNotAtMaxScale;

    private InputAction sprintAction;

    void Awake()
    {
        sprintAction = new InputAction("Sprint", InputActionType.Button, "<Keyboard>/shift");
        sprintAction.performed += OnPress;
        sprintAction.canceled += OnRelease;
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
            OnMaxScaleReached.Invoke();
            sprintFeedback?.PlayFeedbacks();

        }
        else
        {
            OnNotAtMaxScale.Invoke();
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
            .OnComplete(() => OnMaxScaleReached.Invoke());
            
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
            .OnComplete(() => OnNotAtMaxScale.Invoke());
    }

    private void OnEnable()
    {
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        sprintAction.Disable();
    }
    
}
