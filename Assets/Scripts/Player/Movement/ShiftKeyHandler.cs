using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class ShiftKeyHandler : MonoBehaviour
    {
        [Header("Feedbacks")]
        [SerializeField] private MMFeedbacks sprintFeedback;
        [SerializeField] private GameObject walkBody;   // Default body
        [SerializeField] private GameObject sprintBody; // Body when sprinting
        [SerializeField] private Vector3 initialScale = Vector3.one;
        [SerializeField] private Vector3 targetScale;
        [SerializeField] private float pressDuration = 0.26f;
        [SerializeField] private float releaseDuration = 1.51f;
    
        private PlayerMovementV03 _playerMovement;
    
        private PlayerInput _playerInput;
        private InputAction _sprintAction;
    
        private void Awake()
        {
            _playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
                                
            _sprintAction = _playerInput.actions.FindAction("Sprint");
                                
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
            _sprintAction.performed-=OnPress;
            _sprintAction.canceled-=OnRelease;
            _sprintAction.Disable();
        }
        #endregion
        private void Start()
        {
            transform.localScale = initialScale;
            InitialSetup();
        }
        private void InitialSetup()
        {
            OnScale(walkBody);
            ScaleDown(sprintBody);
            _playerMovement.SetCurrentSpeed(_playerMovement.GetMaxRollingSpeed());
        }
    
        public void OnPress(InputAction.CallbackContext context)
        {
            EaseBackDown(walkBody);
            ScaleUp(sprintBody);
            _playerMovement.SetCurrentSpeed(_playerMovement.GetMaxRollingSpeed());
        }
    
        public void OnRelease(InputAction.CallbackContext context)
        {
            if (this == null)
                return;
            OnScale(walkBody);
            ScaleDown(sprintBody);
            _playerMovement.SetCurrentSpeed(_playerMovement.GetMaxFloatingSpeed());
        }
    
        private void OnScale(GameObject body)
        {
            body.transform.DOKill();
            body.transform.DOScale(targetScale, pressDuration).SetEase(Ease.InOutSine);
        }
    
        private void ScaleDown(GameObject body)
        {
            body.transform.DOKill();
            body.transform.DOScale(Vector3.zero, pressDuration).SetEase(Ease.InOutSine);
        }
    
        private void ScaleUp(GameObject body)
        {
            body.transform.DOKill();
            body.transform.DOScale(Vector3.one, releaseDuration).SetEase(Ease.OutBounce);
        }
    
        private void EaseBackDown(GameObject body)
        {
            if (this == null)
            {
                return;
            }
            body.transform.DOKill();
            body.transform.DOScale(initialScale, releaseDuration).SetEase(Ease.OutBounce);
        }
    
    }
}
