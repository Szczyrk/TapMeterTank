using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeChanger : MonoBehaviour
{
    public int minSize = 150;
    public int maxSize = 250;
    public float speed = 25f;

    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.fontSize += Time.deltaTime * speed;

        if (text.fontSize < minSize || text.fontSize > maxSize) speed *= -1;
    }
}
