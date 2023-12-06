using Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class RebindInputUI : MonoBehaviour
    {
        public InputActionReference actionReference;
        private InputActionRebindingExtensions.RebindingOperation _rebindOperation;
        private string _bindingId;

        public void Initialize(InputActionReference actionReference, string bindingId)
        {
            this.actionReference = actionReference;
            this._bindingId = bindingId;

            // Convert bindingId from string to integer
            int bindingIndex = actionReference.action.bindings.IndexOf(x => x.id.ToString() == bindingId);

            // Check if the bindingId exists in the bindings list
            if (bindingIndex < 0)
            {
                Debug.LogError($"Binding with ID {bindingId} does not exist in action {actionReference.action.name}.");
                return;
            }

            // Disable the action before starting the rebind process
            actionReference.action.Disable();

            _rebindOperation = actionReference.action.PerformInteractiveRebinding().WithTargetBinding(bindingIndex);
        }

        public void StartRebindProcess()
        {
            PlayerInputCommandHandler.Instance.StartRebindProcess(actionReference, this);
        }

        public void OnRebindComplete()
        {
            PlayerInputCommandHandler.Instance.OnRebindComplete(actionReference);

            // Re-enable the action after the rebind process is complete
            actionReference.action.Enable();

            _rebindOperation.Start();
        }

        private void OnDestroy()
        {
            _rebindOperation?.Dispose();
        }
    }
}