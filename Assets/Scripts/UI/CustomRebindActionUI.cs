using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class CustomRebindActionUI : MonoBehaviour
    {
        public InputActionReference actionReference;
        [SerializeField]
        internal string bindingId;

        private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;

        private void Start()
        {
            StartRebindProcess();
        }

        public void StartRebindProcess()
        {
            m_RebindOperation?.Dispose();

            // Disable the action before starting the rebind process
            actionReference.action.Disable();

            // Convert bindingId from string to integer
            int bindingIndex = actionReference.action.bindings.IndexOf(x => x.id.ToString() == bindingId);

            m_RebindOperation = actionReference.action.PerformInteractiveRebinding(bindingIndex)
                // Wait for the user to press a button before completing the rebind process
                .OnMatchWaitForAnother(0.1f)
                // Specify a callback to be invoked when the rebind process is complete
                .OnComplete(operation =>
                {
                    // Re-enable the action after the rebind process is complete
                    actionReference.action.Enable();
                    m_RebindOperation = null;
                })
                .Start();
        }
    }
}