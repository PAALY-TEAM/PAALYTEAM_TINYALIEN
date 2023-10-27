using UnityEngine;
using UnityEngine.InputSystem;

namespace Morphing
{
    public class SliderAnimator : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _maxX = 1f;
        [SerializeField] private float _minX = 0f;
        [SerializeField] private Morpher _morpher;
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
            float targetSliderValue = _movementInput ? _maxX : _minX;
            _currentSliderValue = Mathf.Lerp(_currentSliderValue, targetSliderValue, Time.deltaTime * _speed);
            _morpher.SetSlider(_currentSliderValue);
        }
    }
}