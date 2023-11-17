using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement.Commands
{
    public class SprintCommand : Command
    {
        private ShiftKeyHandler _shiftKeyHandler;
        private InputAction _sprintAction;

        public SprintCommand(ShiftKeyHandler shiftKeyHandler)
        {
            this._shiftKeyHandler = shiftKeyHandler;
            _sprintAction = new InputAction(binding: "<Keyboard>/leftShift");
            _sprintAction.performed += ctx => Execute();
            _sprintAction.canceled += ctx => Execute();
            _sprintAction.Enable();
        }

        public override void Execute()
        {
            /*
            if (_sprintAction.triggered)
            {
                OnPerformed(context: _sprintAction.ReadValueAsObject() is InputAction.CallbackContext ? (InputAction.CallbackContext)_sprintAction.ReadValueAsObject() : default);
            }
            else
            {
                OnCancelled(context: _sprintAction.ReadValueAsObject() is InputAction.CallbackContext ? (InputAction.CallbackContext)_sprintAction.ReadValueAsObject() : default);
            }
            */
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            _shiftKeyHandler.OnPress(context);
        }

        private void OnCancelled(InputAction.CallbackContext context)
        {
            _shiftKeyHandler.OnRelease(context);
        }
    }
}