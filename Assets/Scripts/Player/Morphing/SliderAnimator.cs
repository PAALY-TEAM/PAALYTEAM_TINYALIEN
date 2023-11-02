using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Morphing
{
    public class SliderAnimator : MonoBehaviour
    {
        [FormerlySerializedAs("_speed")] [SerializeField] private float speed = 1f;
        [FormerlySerializedAs("_maxX")] [SerializeField] private float maxX = 1f;
        [FormerlySerializedAs("_minX")] [SerializeField] private float minX = 0f;
        [SerializeField] private NoMorpherOnlyDepForColorChange _morpher;
        [SerializeField] private InputActionReference movementAction;

        private bool _movementInput;
        private float _currentSliderValue;

        private void OnEnable()
        {
            movementAction.action.started += OnMovementStarted;
            movementAction.action.canceled += OnMovementCanceled;
            movementAction.action.Enable();
        }

        private void OnDisable()
        {
            movementAction.action.started -= OnMovementStarted;
            movementAction.action.canceled -= OnMovementCanceled;
            movementAction.action.Disable();
        }

        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            _movementInput = true;
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            _movementInput = false;
        }

        private void Update()
        {
            float targetSliderValue = _movementInput ? maxX : minX;
            _currentSliderValue = Mathf.Lerp(_currentSliderValue, targetSliderValue, Time.deltaTime * speed);
            _morpher.SetSlider(_currentSliderValue);
        }
    }
}