using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HideLikeABall : ExtendedBehaviour
{
    public Transform Center;
    public float Duration;
    public Ease EaseType;

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        Vector3 startingPosition = transform.position;

        transform.DOMove(Center.position, Duration)
            .SetEase(EaseType)
            .OnComplete(
                () =>
                {
                    transform.DOMove(startingPosition, Duration)
                    .SetEase(EaseType)
                    .OnComplete(
                        () =>
                        {
                            this.Finish();
                        }
                    );
                }
            );
    }

    protected override void StopExtendedBehaviour()
    {
        transform.DOKill();
        this.Finish();
    }
}
