using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSkillAnimation : ExtendedBehaviour
{
    public float shootDuration;
    public float returnDuration;

    public float towerDistance;
    public float muzzleDistance;

    public GameObject explosionParticle;

    private Tween tween = null;
    private Transform muzzle;

    private void Start()
    {
        base.Start();

        muzzle = transform.GetChild(0);
    }

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        DoShoot();
        explosionParticle.SetActive(true);
    }

    private void DoShoot()
    {
        tween = transform.DOBlendableLocalMoveBy(new Vector3(0,0,-towerDistance), shootDuration);
        tween = muzzle.DOBlendableLocalMoveBy(new Vector3(0, 0, -muzzleDistance), shootDuration + 0.1f).OnComplete( ()=>ReturnToStartPos() );
    }

    private void ReturnToStartPos()
    {
        tween = transform.DOBlendableLocalMoveBy(new Vector3(0, 0, towerDistance), returnDuration);
        tween = muzzle.DOBlendableLocalMoveBy(new Vector3(0, 0, muzzleDistance), returnDuration + 0.1f).
            OnComplete( ()=>
                            {
                                explosionParticle.SetActive(false);
                                this.Finish();
                            });
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
