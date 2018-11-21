using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{

    public GameObject tareget;
    public GameObject obj;

    Vector3 startingPosition;
    Sequence mySequence;

    public float DurationSeconds;
    public Ease EaseType = Ease.Linear;
    public float alpha;
    bool anim = false;
    private float alphaStart;
    Image image;
    Color color;


    // Use this for initialization
    void Start()
    {

        startingPosition = obj.transform.position;
        image = obj.GetComponent<Image>();
        color = image.color;
        alphaStart = 255;

    }

    // Update is called once per frame
    void Update()
    {
        if (!anim)
        {
            mySequence = DOTween.Sequence();
            anim = true;
            
            mySequence.Append(obj.transform.DOMove(tareget.transform.position, DurationSeconds))
                .SetEase(EaseType);
            mySequence.Insert(0, image.DOFade(alpha, DurationSeconds))
                .SetEase(EaseType)
                .OnComplete(
                   () =>
                   {
                       Debug.Log("TEST");
                       FinishAnimation();
                   }
                   );
           
    
        }
    }
    public void FinishAnimation()
    {
        mySequence.Kill();
        color.a = alphaStart;
        image.color = color;
        obj.transform.position = startingPosition;
        anim = false;
    }
}
