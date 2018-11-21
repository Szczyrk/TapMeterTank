using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PrestartAnimation : ExtendedBehaviour
{
    [Tooltip("In which direction to move.")]
    public Direction Direction;

    [Tooltip("How far go back and then forth.")]
    public float Distance = 2;

    [Tooltip("Duration of going back. So the whole behaviour will last two times this value.")]
    public float DurationSeconds = 1;

    [Tooltip("Easing of movement back. And then forth.")]
    public Ease EaseType = Ease.Linear;

    public GameObject smashParticle;

    private Tween tween = null;
    private bool finished = false;

    private Vector3 startingPosition;
    private Transform myTransform;
    public bool end = false;


    protected override void Awake()
    {
        base.Awake();

        myTransform = transform;
        startingPosition = myTransform.localPosition;
    }

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        // remember a starting position
        Vector3 currentLocalPosition = startingPosition;

        // choose a correct direction
        Vector3 directionVector = myTransform.forward;
        if (Direction == Direction.Backward) directionVector = -myTransform.forward;
        if (Direction == Direction.Right) directionVector = myTransform.right;
        if (Direction == Direction.Left) directionVector = -myTransform.right;
        if (Direction == Direction.Up) directionVector = myTransform.up;
        if (Direction == Direction.Down) directionVector = -myTransform.up;

        // start moving
        finished = false;
        tween = null;
        DoMovement(currentLocalPosition, directionVector);
    }

    private void DoMovement(Vector3 startLocalPosition, Vector3 directionVector)
    {
        tween = myTransform.DOLocalMove(startLocalPosition - (directionVector * Distance), DurationSeconds)
            .SetEase(EaseType)
            .OnComplete(
                () =>
                {
                    if (!finished)
                    {
                        finished = true;
                        smashParticle.SetActive(true);
                        CMCameraSwitch.Instance.NextCamera();
                        StartCoroutine(StartMainProcesses());
                    }
                }
            );
    }

    private IEnumerator StartMainProcesses()
    {
        yield return new WaitForSeconds(1f);

        smashParticle.SetActive(false);
        end = true;
        this.Finish();
    }

    protected override void StopExtendedBehaviour()
    {
        if (tween != null)
        {
            tween.Kill();
        }
        
        this.Finish();
        
    }
}
