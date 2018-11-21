using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTowerSkillAnimation : ExtendedBehaviour
{
    public float spinDuration;
    public int spins;

    private Tween tween = null;


    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        DoSpin(spins);
    }

    private void DoSpin(int spinsLeft)
    {
        if (spinsLeft-- > 0)
        {
            tween = transform.DOBlendableLocalRotateBy(new Vector3(0, 360 * spins, 0), spinDuration, RotateMode.FastBeyond360).OnComplete(() => this.Finish());
        }
        else
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
