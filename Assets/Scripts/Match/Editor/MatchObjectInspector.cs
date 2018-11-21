using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MatchObject))]
public class MatchObjectInspector : Editor
{

    public MatchObject _target;

    public Object source;

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        _target = (MatchObject)target;

        if (GUILayout.Button("Set base position, rotation, and scale"))
        {
            SetBasePositionRotationScale(_target);
        }

        if (GUILayout.Button("Set target position, rotation, and scale"))
        {
            SetTargetPositionRotationScale(_target);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Drag and drop controller before setting to controller's current progress value");
        source = EditorGUILayout.ObjectField(source, typeof(MatchController), true);

        if (GUILayout.Button("Move, rotate, and scale according to Tap Controller's progress"))
        {
            MatchController controller = (MatchController)source;
            if (controller != null)
            {
                // Set current target position/rotation/scale
                _target.MakeStep(controller.StartProgress);
                // Move, rotate, and scale
                _target.MoveRotateAndScale(true);
            }
            else
            {
                Debug.LogError("Controller is not set!");
            }
        }

    }

    private void SetBasePositionRotationScale(MatchObject matchObject)
    {
        matchObject.basePosition = matchObject.transform.localPosition;
        matchObject.baseRotation = matchObject.transform.localRotation;
        matchObject.baseScale = matchObject.transform.localScale;
    }

    private void SetTargetPositionRotationScale(MatchObject matchObject)
    {
        matchObject.TargetPosition = matchObject.transform.localPosition;
        matchObject.TargetRotation = matchObject.transform.localRotation;
        matchObject.TargetScale = matchObject.transform.localScale;
    }
}
