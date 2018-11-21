using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchObject : MonoBehaviour {

    // Base position - Game Over position
    [Header("Base position, rotation, and scale")]
    [Tooltip("Base position to which the object returns when player is not tapping")]
    public Vector3 basePosition;
    [Tooltip("Base rotation to which the object returns when player is not tapping")]
    public Quaternion baseRotation = Quaternion.identity;   // Default value is set to avoid errors with NaN quaternions fields
    [Tooltip("Base scale to which the object returns when player is not tapping")]
    public Vector3 baseScale;

    // Target position - Win position
    [Header("Target position, rotation, and scale")]
    [Tooltip("Final position")]
    public Vector3 TargetPosition;
    [Tooltip("Final rotation")]
    public Quaternion TargetRotation = Quaternion.identity;  // Default value is set to avoid errors with NaN quaternions fields
    [Tooltip("Final scale")]
    public Vector3 TargetScale;

    // Animations speed - the "real" object speed is managed by controller
    [Header("Move/Rotate/Scale animation speed")]
    [Tooltip("How fast object will move.")]
    public float PositionLerpValue;
    [Tooltip("How fast object will rotate.")]
    public float RotationLerpValue;
    [Tooltip("How fast object will scale.")]
    public float ScaleLerpValue;

    // Position/Rotation/etc used for smooth transitions
    private Vector3 currentTargetPosition;      // Target position used with Lerp function
    private Quaternion currentTargetRotation;   // ...
    private Vector3 currentTargetScale;         // ...
    private Transform myTransform;              // Keep transform in the local variable to increase performance
    // Property so custom inspector can use it and scripts that use myTransform. Very cool
    public Transform MyTransform { get {
            if (myTransform == null)
            {
                myTransform = transform;
            }
            return myTransform; } }              


    // METHODS

    #region UnityMethods
    void Awake()
    {
        // Keep transform
        myTransform = transform;
    }

    private void Start()
    {
        EventManager.RaiseEventMatchObjectInstantiated(this);

        // Disable, it will be enabled when MatchController will call Restart/Start
        this.enabled = false;
    }

    void Update()
    {
        // Move, rotate, scale
        MoveRotateAndScale();
    }
    #endregion

    public void SetActive(bool active)
    {
        // Turn off Update
        this.enabled = active;
    }

    /// <summary>
    /// Sets position, rotation, and scale to start values. Activates self.
    /// </summary>
    public void ActivateAndSetToCurrentProgress(MatchController controller)
    {
        // Set targets and move object
        if (controller != null)
        {
            // Set current target position/rotation/scale
            MakeStep(controller.CurrentProgress);
            // Set postition/rotation/scale to current Tap Controller's progress
            MoveRotateAndScale(true); // Immediately
        }
        else
        {
            Debug.LogWarning("Controll is null. Set to current position, rotation, and sacle");

            // Set current targets to current position/rotation/scale
            currentTargetPosition = MyTransform.localPosition;
            currentTargetRotation = MyTransform.localRotation;
            currentTargetScale = MyTransform.localScale;
        }

        // Activate
        SetActive(true);
    }

    /// <summary>
    /// Moves, rotates, and scales object to current target values (not final targets!)
    /// </summary>
    public void MoveRotateAndScale(bool moveImmediatelyToTargetPosition = false)
    {
        // Calculate lerv values
        var deltaTime = Time.deltaTime;
        var positionLerp = PositionLerpValue * deltaTime;
        var rotationLerp = RotationLerpValue * deltaTime;
        var scaleLerp = ScaleLerpValue * deltaTime;

        // Move immediately to target position
        if (moveImmediatelyToTargetPosition)
        {
            positionLerp = 1;
            rotationLerp = 1;
            scaleLerp = 1;
        }

        // Move
        MyTransform.localPosition = Vector3.Lerp(MyTransform.localPosition, currentTargetPosition, positionLerp);
        MyTransform.localRotation = Quaternion.Lerp(MyTransform.localRotation, currentTargetRotation, rotationLerp);
        MyTransform.localScale = Vector3.Lerp(MyTransform.localScale, currentTargetScale, scaleLerp);
    }

    /// <summary>
    /// Set new destination point to which object will move/rotate/scale to
    /// </summary>
    /// <param name="positionOnRoad">Value [0,1]. 0 means object is at start point. 1 means object is at final position</param>
    public void MakeStep(float positionOnRoad)
    {
        // Set new point to which object will move/rotate/scale to
        currentTargetPosition = Vector3.Lerp(basePosition, TargetPosition, positionOnRoad);
        currentTargetRotation = Quaternion.Lerp(baseRotation, TargetRotation, positionOnRoad);
        currentTargetScale = Vector3.Lerp(baseScale, TargetScale, positionOnRoad);
    }

}
