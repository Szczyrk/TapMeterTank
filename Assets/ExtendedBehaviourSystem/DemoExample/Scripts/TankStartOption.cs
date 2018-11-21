using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TankStartOption : MonoBehaviour {

    public Tween tween = null
        ;
    [SerializeField]
    [Range(-5, 5)]
    private float speedRotation = .5f;

    [Tooltip("Easing of movement back. And then forth.")]
    public Ease EaseType = Ease.Linear;
    float rot;
    // Update is called once per frame
    void Update () {
        rot += speedRotation;
        tween = transform.DORotateQuaternion(Quaternion.Euler(0, rot, 0), speedRotation);
    }

    public void close()
    {
        tween.Kill();
        GetComponent<TankStartOption>().enabled = false;
    }
}
