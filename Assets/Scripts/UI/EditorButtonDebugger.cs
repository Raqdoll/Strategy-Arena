using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EditorButtonDebugger : MonoBehaviour {

    public UnityEvent debugEvents;


    public void DebuggingEvents()
    {
        debugEvents.Invoke();
    }

#if UNITY_EDITOR
    //using UnityEditor;

    [UnityEditor.CustomEditor(typeof(PlayerMovement))]
    public class LaserButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorButtonDebugger thisScript = (EditorButtonDebugger)target;

            if (GUILayout.Button("Invoke debugging events"))
            {
                thisScript.DebuggingEvents();
            }
        }
    }
#endif

}
