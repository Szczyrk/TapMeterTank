using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveUIPanel : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float durationTime = 1f;

    Tween tween = null;

    private void Start()
    {
        transform.GetComponent<RectTransform>().anchoredPosition = endPosition;
    }

    public void Show()
    {
        tween = transform.GetComponent<RectTransform>().DOLocalMove(startPosition, durationTime);
    }

    public void Hide()
    {
        tween = transform.GetComponent<RectTransform>().DOLocalMove(endPosition, durationTime);
    }
}
