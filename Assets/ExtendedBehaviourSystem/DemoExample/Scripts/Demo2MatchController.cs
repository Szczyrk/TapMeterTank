using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2MatchController : MonoBehaviour {

    [Tooltip("The process that will be repeated during the match.")]
    public SO_process MainNeutralProcess;

    public SO_process MainPlayerProcess;

    public SO_process MainEnemyProcess;

    public SO_process PreStartProcess;


    //Pause all main processes
    public SO_process PauseProcess;

    bool started = false;

    private void Awake()
    {
        EventManager.SubscribeToEventAllProcessesFinished(OnAllProcessesFinished);
        EventManager.SubscribeToEventMatchEnd(OnMatchEnd);
        EventManager.SubscribeToEventMatchRestarted(OnMatchStarted);
        EventManager.SubscribeToEventMatchStarted(OnMatchStarted);
        EventManager.SubscribeToEventMatchPaused(OnMatchPaused);
        EventManager.SubscribeToEventMatchResumed(OnMatchResumed);
    }

    private void OnDestroy()
    {
        EventManager.UnsubscribeFromEventAllProcessesFinished(OnAllProcessesFinished);
        EventManager.UnsubscribeFromEventMatchEnd(OnMatchEnd);
        EventManager.UnsubscribeFromEventMatchRestarted(OnMatchStarted);
        EventManager.UnsubscribeFromEventMatchStarted(OnMatchStarted);
        EventManager.UnsubscribeFromEventMatchPaused(OnMatchPaused);
        EventManager.UnsubscribeFromEventMatchResumed(OnMatchResumed);
    }

    private void OnMatchPaused()
    {
        // TODO: what about players proceses?
        EventManager.RaiseEventMainProcessStarted(PauseProcess);
    }

    private void OnMatchResumed()
    {
        // TODO: what about players proceses?
        EventManager.RaiseEventMainProcessStarted(MainNeutralProcess);
    }

    private void OnMatchEnd(bool won)
    {
        started = false;
        EventManager.RaiseEventProcessStarted(PauseProcess, MatchProgressChangingObject.Type.Player);
    }

    private void OnMatchStarted()
    {
        if (PreStartProcess != null)
            EventManager.RaiseEventProcessStarted(PreStartProcess);

        started = true;
    }

    private void OnAllProcessesFinished(bool isMainProcessWorking, bool isMainPlayerProcessWorking,bool isMainEnemyProcessWorking)
    {
        if (!started) return;
        Debug.Log(isMainProcessWorking.ToString() + isMainPlayerProcessWorking.ToString() + isMainEnemyProcessWorking.ToString());
        if (!isMainProcessWorking && MainNeutralProcess != null)
            EventManager.RaiseEventMainProcessStarted(MainNeutralProcess, MatchProgressChangingObject.Type.Neutral);
        if (!isMainPlayerProcessWorking && MainPlayerProcess != null)
            EventManager.RaiseEventMainProcessStarted(MainPlayerProcess, MatchProgressChangingObject.Type.Player);
        if (!isMainEnemyProcessWorking && MainEnemyProcess != null)
            EventManager.RaiseEventMainProcessStarted(MainEnemyProcess, MatchProgressChangingObject.Type.Enemy);

    }
}
