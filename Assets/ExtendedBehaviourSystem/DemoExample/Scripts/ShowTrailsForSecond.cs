using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTrailsForSecond : ExtendedBehaviour
{
    public float TrailSeconds;
    private TrailRenderer myTrailRenderer;

    protected override void Awake()
    {
        base.Awake();
        myTrailRenderer = GetComponent<TrailRenderer>();
    }

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        myTrailRenderer.emitting = true;
        StartCoroutine(DelayDisableTrail(TrailSeconds));
    }

    private IEnumerator DelayDisableTrail(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        myTrailRenderer.emitting = false;
        this.Finish();
    }

    protected override void StopExtendedBehaviour()
    {
        StopAllCoroutines();
        myTrailRenderer.emitting = false;
        this.Finish();
    }
}
