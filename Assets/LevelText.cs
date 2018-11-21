using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    TextMeshProUGUI text;
    public Image fadeEffect;
    public float fadeSpeed = 0.3f;
    Color currentColor;
    

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Level " + PlayerStatistic.Instance.GetLevelGame().ToString();
        currentColor = fadeEffect.color;
    }

    private void Update()
    {
        currentColor.a += fadeSpeed * Time.deltaTime;
        if (currentColor.a <= 0 || currentColor.a >= 0.7) fadeSpeed *= -1;
        fadeEffect.color = currentColor;
    }
}
