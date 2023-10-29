using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DoorTriggerInteraction))]
    class LabelHandle : UnityEditor.Editor
    {
        private static GUIStyle labelStyle;

        private void OnEnable()
        {
            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.white;
            labelStyle.alignment = TextAnchor.MiddleCenter;
        }

        private void OnSceneGUI()
        {
            DoorTriggerInteraction door = (DoorTriggerInteraction)target;
        
            Handles.BeginGUI();
            Handles.EndGUI();
        }
    }
}

