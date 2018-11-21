using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;


public class SetOnMolotov : ExtendedBehaviour {

    [SerializeField]
    GameObject particals;

    [SerializeField]
    private GameObject item;

    [SerializeField]
    private GameObject hand;

    [SerializeField]
    private GameObject bomber;

    GameObject target;
    NavMeshAgent agent;

    [SerializeField]
    SO_process process;

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
    private Vector3 distanceFromOpponent;

    Vector3[] waypoints;
    [SerializeField]
    int waypointsCount = 5;

    private Tween tween = null;

    private Vector3 bomberStartPosition;

    private float duration = 0f;
    private bool active = false;
    private MatchProgressChangingObject.Type processOwnerType;

    [Tooltip("Easing of movement back. And then forth.")]
    public Ease EaseType = Ease.Linear;

    [Tooltip("Duration of going back. So the whole behaviour will last two times this value.")]
    public float DurationSeconds = 1;
    private bool stopMovement = false;
    private bool holdHand = false;

    Animator animatorBomber;

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
        Debug.Log(bomberStartPosition);
        agent = bomber.GetComponent<NavMeshAgent>();

        if (bomber.GetComponent<Animator>())
        {
            animatorBomber = bomber.GetComponent<Animator>();
        }
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

            animatorBomber.SetFloat("Speed_f", agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);
            if (holdHand)
                item.transform.position = hand.transform.position;
        }
    }

    private void OnDestroy()
    {
    }

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        this.processOwnerType = processOwnerType;

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
        item.transform.position = hand.transform.position;
        holdHand = true;
        agent.SetDestination(new Vector3(transform.position.x + distanceFromOpponent.x, bomberStartPosition.y, transform.position.z + distanceFromOpponent.z));
        StartCoroutine(BomberTime());
    }

    private void Bombing()
    {
        waypoints = new Vector3[waypointsCount];
        for (int i = 0; i < waypointsCount / 2; i++)
        {
            waypoints[i] = item.transform.position + new Vector3(distanceFromOpponent.x * i, distanceFromOpponent.y * i, distanceFromOpponent.z * i);
        }
        for (int i = 0; i < waypointsCount / 2; i++)
        {
            waypoints[i] = item.transform.position + new Vector3(distanceFromOpponent.x * i, -distanceFromOpponent.y * i, distanceFromOpponent.z * i);
        }
        waypoints[waypointsCount - 1] = transform.position;



        holdHand = false;
        tween = item.GetComponent<Rigidbody>().DOPath(waypoints, DurationSeconds, PathType.Linear, PathMode.Full3D, 10, Color.green);
        agent.SetDestination(bomberStartPosition);
        StartCoroutine(TweenTime()); // Drill Ground hit

    }

    IEnumerator BomberTime()
    {
        while (agent.isStopped == true)
        {
            yield return null;
        }
        item.SetActive(true);
        if (animatorBomber)
        {


            animatorBomber.StopPlayback();
            animatorBomber.SetInteger("WeaponType_int", 10);
            StartCoroutine(GrenadeThrowTime());
        }
        else
            Bombing();
    }

    IEnumerator GrenadeThrowTime()
    {
        yield return new WaitForSeconds(2.5f);
        if (animatorBomber)
        {
            animatorBomber.SetInteger("WeaponType_int", 1);
        }
        Bombing();
    }

    IEnumerator TweenTime()
    {
        yield return new WaitForSeconds(DurationSeconds);
        if (particals)
            particals.SetActive(true);

        StartCoroutine(CallStopBehaviour());
    }

    IEnumerator CallStopBehaviour()
    {
        while (agent.isStopped == true)
        {
            yield return null;
        }
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
        if (particals)
        { 
            particals.SetActive(false);
            EventManager.RaiseEventProcessStarted(process, processOwnerType);
        }

        if (tween != null)
        {
            tween.Kill();
        }

        this.Finish();
    }

    private IEnumerator disableHealthText(SO_effect effect)
    {
        yield return new WaitForSeconds(effect.IntervalDurationSeconds - effect.IntervalDurationSeconds / 2);
    }
}


