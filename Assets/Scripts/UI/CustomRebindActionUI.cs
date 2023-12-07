using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UI
{
    public class CustomRebindActionUI : MonoBehaviour
    {
        public InputActionReference actionReference;
        [SerializeField]
        internal string bindingId;
        
        

        public Text actionLabel;
private Text m_ActionLabel
        {
            get => actionLabel;
            set => actionLabel = value;
        }
        
        public Text rebindPrompt;
        private Text m_RebindText
        {
            get => rebindPrompt;
            set => rebindPrompt = value;
        }

        public InteractiveRebindEvent startRebindEvent;
        private InteractiveRebindEvent m_RebindStartEvent
        {
            get => startRebindEvent;
            set => startRebindEvent = value;
        }

        public Text bindingText;
        private Text m_BindingText
        {
            get => bindingText;
            set => bindingText = value;
        }
        
        public GameObject rebindOverlay;
        private GameObject m_RebindOverlay
        {
            get => rebindOverlay;
            set => rebindOverlay = value;
        }
        public InteractiveRebindEvent stopRebindEvent;
        private InteractiveRebindEvent m_RebindStopEvent
        {
            get => stopRebindEvent;
            set => stopRebindEvent = value;
        }

        
        
        public UpdateBindingUIEvent updateBindingUIEvent;
        
        private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;
        private int bindingIndex;
        
        
        private void Start()
        {
            StartRebindProcess();
        }

        public void StartRebindProcess()
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex))
                return;

            // If the binding is a composite, we need to rebind each part in turn.
            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
            }
            else
            {
                PerformInteractiveRebind(action, bindingIndex);
            }
            
            m_RebindOperation?.Dispose();

            // Check if actionReference or actionReference.action is null
            if (actionReference == null || actionReference.action == null)
            {
                Debug.LogError("actionReference or actionReference.action is null");
                return;
            }

            // Disable the action before starting the rebind process
            actionReference.action.Disable();

            // Convert bindingId from string to integer
            // Remove the 'int' keyword to use the class-level 'bindingIndex' variable
            bindingIndex = actionReference.action.bindings.IndexOf(x => x.id.ToString() == bindingId);
            
            // Check if bindingIndex is -1
            if (bindingIndex == -1)
            {
                Debug.LogError("No binding found with the specified ID");
                return;
            }
            
            m_RebindOperation = actionReference.action.PerformInteractiveRebinding(bindingIndex)
                // Wait for the user to press a button before completing the rebind process
                .OnMatchWaitForAnother(0.1f)
                // Specify a callback to be invoked when the rebind process is complete
                .OnComplete(operation =>
                {
                    // Re-enable the action after the rebind process is complete
                    actionReference.action.Enable();
                    m_RebindOperation = null;
                    stopRebindEvent.Invoke(this, operation);
                })
                .Start();

            startRebindEvent.Invoke(this, m_RebindOperation);
        }

        private void Update()
        {
            // Check if m_RebindOperation is null
            if (m_RebindOperation == null)
            {
                Debug.LogError("m_RebindOperation is null");
                return;
            }

            if (m_RebindOperation.started)
            {
                rebindOverlay.SetActive(true);
                rebindPrompt.text = "Press any key...";
            }
            else
            {
                rebindOverlay.SetActive(false);
                rebindPrompt.text = "";
            }

            // Check if actionReference or actionReference.action is null
            if (actionReference == null || actionReference.action == null)
            {
                Debug.LogError("actionReference or actionReference.action is null");
                return;
            }

            string displayString = actionReference.action.GetBindingDisplayString(bindingIndex);
            bindingText.text = displayString;
            updateBindingUIEvent.Invoke(this, displayString, "", "");
        }
        public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
        {
            bindingIndex = -1;

            action = actionReference?.action;
            if (action == null)
                return false;

            if (string.IsNullOrEmpty(bindingId))
                return false;

            // Look up binding index.
            var bindingIdGuid = new Guid(bindingId);
            bindingIndex = action.bindings.IndexOf(x => x.id == bindingIdGuid);
            if (bindingIndex == -1)
            {
                Debug.LogError($"Cannot find binding with ID '{bindingIdGuid}' on '{action}'", this);
                return false;
            }

            return true;
        }
        private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            m_RebindOperation?.Cancel(); // Will null out m_RebindOperation.

            void CleanUp()
            {
                m_RebindOperation?.Dispose();
                m_RebindOperation = null;
            }

            // Configure the rebind.
            m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .OnCancel(
                    operation =>
                    {
                        m_RebindStopEvent?.Invoke(this, operation);
                        m_RebindOverlay?.SetActive(false);
                        UpdateBindingDisplay();
                        CleanUp();
                    })
                .OnComplete(
                    operation =>
                    {
                        m_RebindOverlay?.SetActive(false);
                        m_RebindStopEvent?.Invoke(this, operation);
                        UpdateBindingDisplay();
                        CleanUp();

                        // If there's more composite parts we should bind, initiate a rebind
                        // for the next part.
                        if (allCompositeParts)
                        {
                            var nextBindingIndex = bindingIndex + 1;
                            if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                PerformInteractiveRebind(action, nextBindingIndex, true);
                        }
                    });

            // If it's a part binding, show the name of the part in the UI.
            var partName = default(string);
            if (action.bindings[bindingIndex].isPartOfComposite)
                partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

            // Bring up rebind overlay, if we have one.
            m_RebindOverlay?.SetActive(true);
            if (m_RebindText != null)
            {
                var text = !string.IsNullOrEmpty(m_RebindOperation.expectedControlType)
                    ? $"{partName}Waiting for {m_RebindOperation.expectedControlType} input..."
                    : $"{partName}Waiting for input...";
                m_RebindText.text = text;
            }

            // If we have no rebind overlay and no callback but we have a binding text label,
            // temporarily set the binding text label to "<Waiting>".
            if (m_RebindOverlay == null && m_RebindText == null && m_RebindStartEvent == null && m_BindingText != null)
                m_BindingText.text = "<Waiting...>";

            // Give listeners a chance to act on the rebind starting.
            m_RebindStartEvent?.Invoke(this, m_RebindOperation);

            m_RebindOperation.Start();
        }

        public void UpdateBindingDisplay()
        {
            var displayString = string.Empty;
            var deviceLayoutName = default(string);
            var controlPath = default(string);

            // Get display string from action.
            var action = actionReference?.action;
            if (action != null)
            {
                var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == bindingId);
                if (bindingIndex != -1)
                    displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, new InputBinding.DisplayStringOptions());
            }

            // Set on label (if any).
            if (bindingText != null)
                bindingText.text = displayString;

            // Give listeners a chance to configure UI in response.
            updateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
        }
        
        [Serializable]
        public class UpdateBindingUIEvent : UnityEvent<CustomRebindActionUI, string, string, string> { }

        [Serializable]
        public class InteractiveRebindEvent : UnityEvent<CustomRebindActionUI, InputActionRebindingExtensions.RebindingOperation> { }
    }
}
