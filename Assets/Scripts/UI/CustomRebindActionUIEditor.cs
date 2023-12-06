using System;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(CustomRebindActionUI))]
public class CustomRebindActionUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CustomRebindActionUI myTarget = (CustomRebindActionUI)target;

        // Draw the default inspector
        DrawDefaultInspector();

        // Get the action from the action reference
        var action = myTarget.actionReference.action;

        // Create a dropdown for the binding options
        if (action != null)
        {
            var bindings = action.bindings;
            var options = new string[bindings.Count];
            for (int i = 0; i < bindings.Count; i++)
            {
                options[i] = bindings[i].ToDisplayString();
            }

            int currentSelected = Array.IndexOf(options, myTarget.bindingId);
            int newSelected = EditorGUILayout.Popup("Binding", currentSelected, options);

            // Update the bindingId when a new option is selected
            if (newSelected != currentSelected)
            {
                myTarget.bindingId = options[newSelected];
            }
        }
    }
}