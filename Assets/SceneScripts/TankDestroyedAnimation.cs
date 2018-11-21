using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDestroyedAnimation : ExtendedBehaviour
{
    public Transform muzzle;
    public Transform caterpillarLeft;
    public Transform caterpillarRight;

    public float bodyTargetHeight;

    private Tween tween = null;

    private void Start()
    {

        base.Start();
        //EventManager.SubscribeToEventMatchRestarted(Restart());
    }

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        DoCaterpllars();
        DoBody();
        DoMuzzle();
    }

    void DoCaterpllars()
    {
        tween = caterpillarLeft.DOLocalRotate(new Vector3(0f, 0f, 90f), 1f);
        tween = caterpillarRight.DOLocalRotate(new Vector3(0f, 0f, -90f), 1f);
    }

    private void DoBody()
    {
        tween = transform.DOLocalMoveY(bodyTargetHeight, 0.5f);
        tween = transform.DOLocalRotate(transform.eulerAngles + new Vector3(2, 0, -3), 1.5f).OnComplete(
                                    () =>
                                    {
                                            this.Finish();
                                        if (tween != null)
                                        {
                                            tween.Kill();
                                        }
                                    }
                                );
    }

    private void DoMuzzle()
    {
        tween = muzzle.DOLocalRotate(new Vector3(30f, 0, 0) , 0.5f);
    }

    private void Restart()
    {
        
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
