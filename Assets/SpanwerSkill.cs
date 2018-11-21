using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpanwerSkill : MonoBehaviour
{
    [SerializeField]
    Vector3 pointStart;

    [SerializeField]
    List<ExtendedBehaviour> skillsSpawn;

    List<SO_behaviour> skillsToUse;

    Vector3[] waypoints;
    [SerializeField]
    int waypointsCount = 5;

    Tween tween;

    [SerializeField]
    float speed;

    [SerializeField]
    float speedSpawn;

    bool spawned = false;
    // Use this for initialization
    void Start()
    {

    }

    void SpawnRandomObject()
    {
        //spawns item in array position between 0 and 100
        int whichItem = Random.Range(0, skillsSpawn.Count);

        GameObject buttonSpawn = Instantiate(skillsSpawn[whichItem].buttonSpawn.gameObject) as GameObject;
        RectTransform rectTransform = buttonSpawn.GetComponent<RectTransform>();
        rectTransform.position = pointStart;

        waypoints = new Vector3[waypointsCount];

        for (int i = 0; i < waypointsCount; i++)
        {
            waypoints[i] = rectTransform.position + new Vector3(Random.Range(-1,1), Random.Range(-1, -0.1f), rectTransform.position.z);
        }

        tween = rectTransform.DOPath(waypoints, speed, PathType.Linear, PathMode.Full3D, 10, Color.green);

        //skillsToUse.Add(skillsSpawn[whichItem].Behaviour);
        StartCoroutine(Destroy(buttonSpawn));
    }

    void Update()
    {
        if (!spawned)
        {
            spawned = true;
            StartCoroutine(Time());
        }

    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(speedSpawn);
        SpawnRandomObject();
    }
    IEnumerator Destroy(GameObject buttonSpawn)
    {
        yield return new WaitForSeconds(speed);
        Destroy(buttonSpawn);
        if(tween != null)
        {
            tween.Kill();
        }
        spawned = false;
    }
}
