using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SetOnDrop : ExtendedBehaviour
{
    [SerializeField]
    private GameObject particals;

    [SerializeField]
    private GameObject item;

    [SerializeField]
    private GameObject bomber;


    private Transform enemyObject;

    private Transform playerObject;

    [SerializeField]
    [Range(-5, 5)]
    private float zOffset = 0f;

    [SerializeField]
    [Range(-5, 5)]
    private float yOffset = 0f;

    [SerializeField]
    [Range(-5, 5)]
    private float xOffset = 0f;

    [SerializeField]
    [Range(0, 10)]
    private float height = 0f;

    [SerializeField]
    [Range(-5, 5)]
    private float finallyheight = 0f;

    private Tween tween = null;
    private Tween tween_bomber = null;

    private Vector3 bomberStartPosition;

    private float duration = 0f;
    private bool active = false;
    private MatchProgressChangingObject.Type processOwnerType;

    [Tooltip("Easing of movement back. And then forth.")]
    public Ease EaseType = Ease.Linear;

    [Tooltip("Duration of going back. So the whole behaviour will last two times this value.")]
    public float DurationSeconds = 1;
    public float DurationSecondsBomber = 1;
    private int r = -1;
    private bool stopMovement = false;

    public bool end;

    protected override void Start()
    {
        base.Start();

        if (item)
        {
            var particles = particals.GetComponent<ParticleSystem>();
            if (particles)
            {
                duration = particles.main.duration;
            }
        }

        enemyObject = GameObject.FindGameObjectWithTag("Enemy").transform;
        playerObject = GameObject.FindGameObjectWithTag("Player").transform;

        bomberStartPosition = bomber.transform.position;
       
    }

    private void Update()
    {
        if (active)
        {
            if (processOwnerType.Equals(MatchProgressChangingObject.Type.Player))
            {
                transform.position = new Vector3(enemyObject.position.x + xOffset, enemyObject.position.y + yOffset, enemyObject.position.z + zOffset);
            }
            else if (processOwnerType == MatchProgressChangingObject.Type.Enemy)
            {
                transform.position = new Vector3(playerObject.position.x + xOffset, playerObject.position.y + yOffset, playerObject.position.z + zOffset);
            }
        }
    }

    private void OnDestroy()
    {
    }

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        this.processOwnerType = processOwnerType;
        end = false;
        if (!active)
        if (duration > 0f)
        {
            active = true;
            stopMovement = false;
            AirRaid();
        }
        else
        {
            StopExtendedBehaviour();
        }
    }

    private void AirRaid()
    {
        if (stopMovement) return;
        if (processOwnerType.Equals(MatchProgressChangingObject.Type.Player))
            transform.position = new Vector3(enemyObject.position.x + xOffset, enemyObject.position.y + yOffset, enemyObject.position.z + zOffset);
        else if (processOwnerType == MatchProgressChangingObject.Type.Enemy)
            transform.position = new Vector3(playerObject.position.x + xOffset, playerObject.position.y + yOffset, playerObject.position.z + zOffset);
        bomber.SetActive(true);
        item.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        tween_bomber = bomber.transform.DOMove(item.transform.position, DurationSecondsBomber)
            .SetEase(EaseType);
                        StartCoroutine(BomberTime());
    }

    private void Bombing()
    {
        item.SetActive(true);
        tween_bomber = bomber.transform.DOMove(new Vector3(r * bomberStartPosition.x, bomberStartPosition.y, r * bomberStartPosition.z), DurationSeconds)
             .SetEase(EaseType);
        tween = item.transform.DOMove(new Vector3(transform.position.x, transform.position.y + finallyheight, transform.position.z), DurationSeconds)
            .SetEase(EaseType);
        item.transform.rotation = Quaternion.Euler(0, 180 - r * 90, item.transform.rotation.z);
        tween = item.transform.DORotateQuaternion(Quaternion.Euler(-90,90, item.transform.rotation.z), DurationSeconds);
        StartCoroutine(TweenTime()); // Drill Ground hit
        
    }

    IEnumerator BomberTime()
    {
        yield return new WaitForSeconds(DurationSecondsBomber);
        Bombing();
    }

    IEnumerator TweenTime()
    {
        yield return new WaitForSeconds(DurationSeconds);
        tween_bomber = bomber.transform.DORotateQuaternion(Quaternion.AngleAxis(180 - r * 90,Vector3.up), DurationSecondsBomber);
        r *= -1;
        particals.SetActive(true);
        StartCoroutine(CallStopBehaviour());
    }

    IEnumerator CallStopBehaviour()
    {
        yield return new WaitForSeconds(duration);
        StopExtendedBehaviour();
    }

    protected override void StopExtendedBehaviour()
    {
        if (item)
        {
            item.SetActive(false);
            stopMovement = true;
            bomber.SetActive(false);
            StopAllCoroutines();
            active = false;
        }
        if(particals)
            particals.SetActive(false);

        if (tween != null)
        {
            tween.Kill();
        }
        if (tween_bomber != null)
        {
            tween_bomber.Kill();
        }
        end = true;
        this.Finish();
    }

    private IEnumerator disableHealthText(SO_effect effect)
    {
        yield return new WaitForSeconds(effect.IntervalDurationSeconds - effect.IntervalDurationSeconds / 2);
    }
}
