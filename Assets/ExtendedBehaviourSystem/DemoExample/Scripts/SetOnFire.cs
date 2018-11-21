using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetOnFire : ExtendedBehaviour
{
    [SerializeField]
    private GameObject fire;

    private Transform enemyObject;

    private Transform playerObject;

    [SerializeField]
    private SO_effect healthDecreaseEffect;

    [SerializeField]
    private TextMesh healthText;

    [SerializeField]
    [Range(-5, 5)]
    private float zOffset = 0f;

    [SerializeField]
    [Range(-5, 5)]
    private float yOffset = 0f;

    [SerializeField]
    [Range(-5, 5)]
    private float xOffset = 0f;


    private float duration = 0f;
    private bool active = false;
    private MatchProgressChangingObject.Type processOwnerType;

    protected override void Start()
    {
        base.Start();

        if (fire)
        {
            var particles = fire.GetComponent<ParticleSystem>();
            if (particles)
            {
                duration = particles.main.duration;
            }
        }

        enemyObject = GameObject.FindGameObjectWithTag("Enemy").transform;
        playerObject = GameObject.FindGameObjectWithTag("Player").transform;

        if (healthDecreaseEffect != null)
            EventManager.SubscribeToEventEffectApplied(onHealthDecreased);
    }

    private void OnDestroy()
    {
        if (healthDecreaseEffect != null)
            EventManager.UnsubscribeFromEventEffectApplied(onHealthDecreased);
    }

    private void Update()
    {
        if (active)
        {
            if(processOwnerType.Equals(MatchProgressChangingObject.Type.Player))
                transform.position = new Vector3(enemyObject.position.x + xOffset, enemyObject.position.y + yOffset, enemyObject.position.z + zOffset);
            else if(processOwnerType == MatchProgressChangingObject.Type.Enemy)
                transform.position = new Vector3(playerObject.position.x + xOffset, playerObject.position.y + yOffset, playerObject.position.z + zOffset);

        }
    }


    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        this.processOwnerType = processOwnerType;
        if (duration > 0f)
        {
            if (processOwnerType.Equals(MatchProgressChangingObject.Type.Player))
                transform.position = new Vector3(enemyObject.position.x + xOffset, enemyObject.position.y + yOffset, enemyObject.position.z + zOffset);
            else if (processOwnerType == MatchProgressChangingObject.Type.Enemy)
                transform.position = new Vector3(playerObject.position.x + xOffset, playerObject.position.y + yOffset, playerObject.position.z + zOffset);

            fire.SetActive(true);
            active = true;
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
        if (fire)
        {
            fire.SetActive(false);
            healthText.gameObject.SetActive(false);
            StopAllCoroutines();
            active = false;
        }

        this.Finish();
    }

    private void onHealthDecreased(SO_effect effect)
    {
        if(effect.name.Equals(healthDecreaseEffect.name))
        {
                Debug.Log("Health Decreased" + effect.ChangeValue);
                healthText.text = effect.ChangeValue.ToString() + " HP";
                healthText.gameObject.SetActive(true);
                StartCoroutine(disableHealthText(effect));
            
        }
    }

    private IEnumerator disableHealthText(SO_effect effect)
    {
        yield return new WaitForSeconds(effect.IntervalDurationSeconds - effect.IntervalDurationSeconds / 2);
        healthText.gameObject.SetActive(false);
    }
}
