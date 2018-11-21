using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class AnimationClick : MonoBehaviour {

    private Tween tween = null;

    public float DurationSeconds = 1;

    [Tooltip("Easing of movement back. And then forth.")]
    public Ease EaseType = Ease.Linear;

    public Vector3 scale;
    public float alpha;

    private Vector3 scaleStart;
    private float alphaStart;
    private float materialStart;

    Image image;
    Material material;

    public bool colorIs;
    public bool scaleIs;
    public bool materialIs;


    private void Awake()
    {
        if(colorIs)
        {
            image = GetComponent<Image>();
            alphaStart = image.color.a;
        }
        if (scaleIs)
            scaleStart = transform.localScale;
        if(materialIs)
        {
            material = GetComponent<Material>();
            materialStart = material.color.a;
        }

        
        DoAnimation(); 
    }

    private void DoAnimation()
    {

        if (scaleIs && colorIs)
        {
            tween = transform.DOScale(scaleStart + scale, DurationSeconds).SetEase(EaseType);
            tween = image.DOFade(alpha, DurationSeconds)
           .OnComplete(
               () =>
               {
               // and "forth", means return to the original local position
                   tween = transform.DOScale(scaleStart, DurationSeconds).SetEase(EaseType).SetEase(EaseType);
                   tween = image.DOFade(alphaStart, DurationSeconds)
                   .OnComplete(
                          () =>
                          {
                              DoAnimation();
                          }
                      );
               }
           );
        }
        if (scaleIs && materialIs)
        {
            tween = transform.DOScale(scaleStart + scale, DurationSeconds).SetEase(EaseType);
            tween = material.DOFade(alpha, DurationSeconds)
           .OnComplete(
               () =>
               {
                   // and "forth", means return to the original local position
                   tween = transform.DOScale(scaleStart, DurationSeconds).SetEase(EaseType).SetEase(EaseType);
                   tween = material.DOFade(materialStart, DurationSeconds)
                   .OnComplete(
                          () =>
                          {
                              DoAnimation();
                          }
                      );
               }
           );
        }
        if (scaleIs)
        {
            tween = transform.DOScale(scaleStart + scale, DurationSeconds).SetEase(EaseType)
           .OnComplete(
               () =>
               {
                   // and "forth", means return to the original local position
                   tween = transform.DOScale(scaleStart, DurationSeconds).SetEase(EaseType).SetEase(EaseType)
                   .OnComplete(
                          () =>
                          {
                              DoAnimation();
                          }
                      );
               }
           );
        }
        if (colorIs)
        {
            tween = image.DOFade(alpha, DurationSeconds)
           .OnComplete(
               () =>
               {
                   // and "forth", means return to the original local position
                   tween = image.DOFade(alphaStart, DurationSeconds)
                   .OnComplete(
                          () =>
                          {
                              DoAnimation();
                          }
                      );
               }
           );
        }
    }

}
