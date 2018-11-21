using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMatchController : MonoBehaviour
{
    public GameObject PrefabPlayer;
    public GameObject PrefabEnemy;

    [Tooltip("The process that will be repeated during the match.")]
    public SO_process MainBasicProcess;

    [Tooltip("Demo example of a hit process.")]
    public SO_process HitThePlayer;

    [Tooltip("Demo example of another process/skill.")]
    public SO_process HideLikeTurtle;

    private void Awake()
    {
        EventManager.SubscribeToEventAllProcessesFinished(OnAllProcessesFinished);
    }

    private void OnDestroy()
    {
        EventManager.UnsubscribeFromEventAllProcessesFinished(OnAllProcessesFinished);
    }

    private void OnAllProcessesFinished(bool isMainProcessWorking, bool isMainPlayerProcessWorking, bool isMainEnemyProcessWorking)
    {
        if (!isMainProcessWorking)
        {
           EventManager.RaiseEventMainProcessStarted(MainBasicProcess);
        }
    }

    IEnumerator Start()
    {
        if (PrefabPlayer != null)
        {
            Instantiate(PrefabPlayer);
        }

        if (PrefabEnemy != null)
        {
            Instantiate(PrefabEnemy);
        }

        yield return null;
        yield return null;

        EventManager.RaiseEventMainProcessStarted(MainBasicProcess);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            StartProcess(HitThePlayer);
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            StartProcess(HideLikeTurtle);
        }

    }

    public void StartProcess(SO_process process)
    {
        if (process == null) return;

        EventManager.RaiseEventProcessStarted(process);
    }
}
