using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFireworks : ExtendedBehaviour
{
    [SerializeField]
    private GameObject fireworks;

    private float duration = 0f;

    protected override void Start()
    {
        base.Start();

        if (fireworks)
        {
            var particles = fireworks.GetComponent<ParticleSystem>();
            if (particles)
            {
                duration = particles.main.duration;
            }
        }
    }

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        if (duration > 0f)
        {
            fireworks.SetActive(true);
            StartCoroutine(CallStopBehaviour());
        }
        else
        {
            StopExtendedBehaviour();
        }
    }

    IEnumerator CallStopBehaviour()
    {
        yield return new WaitForSeconds(duration);
        StopExtendedBehaviour();
    }

    protected override void StopExtendedBehaviour()
    {
        if (fireworks)
        {
            fireworks.SetActive(false);
        }

        this.Finish();
    }
}
