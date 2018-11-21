using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationHand : MonoBehaviour {

    private Tween tween = null;

    public float Distance = 2;
    public float DurationSeconds = 1;

    [Tooltip("Easing of movement back. And then forth.")]
    public Ease EaseType = Ease.Linear;
    private Transform myTransform;
    public GameObject directionVector;
    private Vector3 startingPosition;

    // Use this for initialization
    void Start () {
        
        
    }
    private void Awake()
    {
        myTransform = transform;
        startingPosition = myTransform.localPosition;

        Vector3 currentLocalPosition = startingPosition;

        DoMovement(currentLocalPosition, directionVector.transform.position);

    }

    void DoMovement(Vector3 startLocalPosition, Vector3 directionVector)
    {
        tween = myTransform.DOLocalMove(startingPosition - (directionVector * Distance), DurationSeconds)
       .SetEase(EaseType)
       .OnComplete(
           () =>
           {
                       // and "forth", means return to the original local position
                       tween = myTransform.DOLocalMove(startingPosition, DurationSeconds)
                          .SetEase(EaseType)
                          .OnComplete(
                              () =>
                       {
                           DoMovement(startingPosition, directionVector);
                       }
                          );
           }
       );
    }
}
