using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProcessController : MonoBehaviour
{
    /// <summary>
    /// The class for internal ProcessController use only - extends info about a process.
    /// </summary>
    private class ProcessInfo
    {
        public SO_process Process;
        public MatchProgressChangingObject.Type OwnerType;
        public List<SO_behaviour> PendingBehaviours;
        public bool IsMainProcess;
    }

    public ExtendedBehaviourController ExtendedBehaviourController;

    // The process that plays throughout the whole match for each MatchProgressChangingObject.Type. 
    //Will be paused on any process with StopsMainPlayerProcess, StopsMainEnemyProcess or stops MainNeutralProcess set to true. 
    //If null then nothing is paused.
    private SO_process mainNeutralProcess;
    private SO_process mainPlayerProcess;
    private SO_process mainEnemyProcess;

    private bool isMainNeutralProcessWorking = false;
    private bool isMainPlayerProcessWorking = false;
    private bool isMainEnemyProcessWorking = false;

    private IEffectApplicable player;
    private IEffectApplicable enemy;

    private LinkedList<ProcessInfo> waitingProcesses = new LinkedList<ProcessInfo>();
    private List<ProcessInfo> currentProcesses = new List<ProcessInfo>();

    private void Awake()
    {
        EventManager.SubscribeToEventProcessStarted(OnProcessStarted);
        EventManager.SubscribeToEventMainProcessStarted(OnMainProcessStarted);
        EventManager.SubscribeToEventAllBehavioursPlayed(OnAllBehavioursPlayed);
        EventManager.SubscribeToEventEffectApplicablePlayerInstantiated(OnPlayerInstantiated);
        EventManager.SubscribeToEventEffectApplicableEnemyInstantiated(OnEnemyInstantiated);
    }

    private void OnDestroy()
    {
        EventManager.UnsubscribeFromEventProcessStarted(OnProcessStarted);
        EventManager.UnsubscribeFromEventMainProcessStarted(OnMainProcessStarted);
        EventManager.UnsubscribeFromEventAllBehavioursPlayed(OnAllBehavioursPlayed);
        EventManager.UnsubscribeFromEventEffectApplicablePlayerInstantiated(OnPlayerInstantiated);
        EventManager.UnsubscribeFromEventEffectApplicableEnemyInstantiated(OnEnemyInstantiated);
    }

    private void OnProcessStarted(SO_process process, MatchProgressChangingObject.Type ownerType)
    {
        ProcessStarted(process, false, ownerType);
    }

    private void OnMainProcessStarted(SO_process process, MatchProgressChangingObject.Type ownerType)
    {
        ProcessStarted(process, true, ownerType);
    }

    private void OnPlayerInstantiated(IEffectApplicable playerObject)
    {
        player = playerObject;
    }

    private void OnEnemyInstantiated(IEffectApplicable enemyObject)
    {
        enemy = enemyObject;
    }


    /// <summary>
    /// Handles events about ProcessStarted. Creates a new ProcessInfo object and executes the process or adds it to the queue.
    /// </summary>
    /// <param name="process">Process that started.</param>
    /// <param name="isMainProcess">Tells if this is a main process.</param>
    /// <param name="processOwner"> Who started the process Player/Enemy/Neutral</param>
    private void ProcessStarted(SO_process process, bool isMainProcess = false, MatchProgressChangingObject.Type processOwner = MatchProgressChangingObject.Type.Neutral)
    {
        ProcessInfo newProcessInfo = new ProcessInfo() {
            Process = process,
            OwnerType = processOwner,
            PendingBehaviours = new List<SO_behaviour>(),
            IsMainProcess = isMainProcess };


        if (process.IgnoresQueue)
        {
            ExecuteProcess(newProcessInfo);
        }
        else
        {
            waitingProcesses.AddLast(newProcessInfo);
            TryExecuteNextProcessFromQueue();
        }
    }

    private void OnAllBehavioursPlayed(SO_behaviour behaviour)
    {
        if (currentProcesses.Count < 1) return;

        // check currentProcesses to find behaviours left to complete some process
        for (int i = 0; i < currentProcesses.Count; i++)
        {
            if (currentProcesses[i].PendingBehaviours.Contains(behaviour))
            {
                // mark the behaviour as completed == remove from a pending list
                currentProcesses[i].PendingBehaviours.Remove(behaviour);
                break;
            }
        }

        //// apply effects from the finished process (it can only be a one process, because only one behaviour kind finished)
        //for (int i = 0; i < currentProcesses.Count; i++)
        //{
        //    if (currentProcesses[i].PendingBehaviours.Count < 1)
        //    {
        //        // if it will not be pausable it means that effects were already applied at the start of the process
        //        if (currentProcesses[i].Process.StopsMainNeutralProcess 
        //            || currentProcesses[i].Process.StopsMainOwnerProcess 
        //            || currentProcesses[i].Process.StopsMainOpponentProcess)
        //        {
        //            // CODE MOVED TO LINE 317, because when enemy frozen (pause enemy main process) and effect decrease health applied,
        //            // hits were counted after freeze process ended
        //        }
        //    }
        //}

        // removed finished processes and check out if main process started
        for (int i = currentProcesses.Count - 1; i >= 0; i--)
        {
            if (currentProcesses[i].PendingBehaviours.Count < 1)
            {
                if (currentProcesses[i].IsMainProcess)
                {
                    switch (currentProcesses[i].OwnerType) {
                        case MatchProgressChangingObject.Type.Neutral:
                            mainNeutralProcess =  currentProcesses[i].Process;
                            isMainNeutralProcessWorking = true;
                            break;
                        case MatchProgressChangingObject.Type.Player:
                            mainPlayerProcess = currentProcesses[i].Process;
                            isMainPlayerProcessWorking = true;
                            break;
                        case MatchProgressChangingObject.Type.Enemy:
                            mainEnemyProcess = currentProcesses[i].Process;
                            isMainEnemyProcessWorking = true;
                            break;

                    }
                }
                currentProcesses.RemoveAt(i);
            }
        }

        // raise event if all processes finished and there is no more waiting
        if (currentProcesses.Count < 1 && waitingProcesses.Count < 1)
        {
            EventManager.RaiseEventAllProcessesFinished(isMainNeutralProcessWorking, isMainPlayerProcessWorking, isMainEnemyProcessWorking);
        }
        else
        {
            // we have to do it eventually
            TryExecuteNextProcessFromQueue();
        }
    }

    /// <summary>
    /// Tries to call ExecuteProcess() on the next available process from the waitingProcesses queue.
    /// </summary>
    private void TryExecuteNextProcessFromQueue()
    {
        // return if no process waiting
        if (waitingProcesses.Count < 1) return;
        // return if some process is still running
        if (currentProcesses.Count > 0) return;

        // get the next process
        var nextProcessToExecute = waitingProcesses.First.Value;
        waitingProcesses.RemoveFirst();

        // execute
        ExecuteProcess(nextProcessToExecute);
    }

    /// <summary>
    /// Executes the given process. Also remembers if this is the main process.
    /// May delay a process execution if there is a need to stop the main process first.
    /// </summary>
    private void ExecuteProcess(ProcessInfo processInfo)
    {
        List<ProcessInfo> processesToStop = new List<ProcessInfo>();

        // prevent to execute the same process multiple times at once
        for (int i = 0; i < currentProcesses.Count; i++)
        {
            if (currentProcesses[i].Process == processInfo.Process)
            {
                // return to the queue
                waitingProcesses.AddFirst(processInfo);

                return;
            }
        }

        // execute depend on if pauses the game or not
        if (processInfo.Process.StopsMainNeutralProcess || processInfo.Process.StopsMainOpponentProcess || processInfo.Process.StopsMainOwnerProcess)
        {
            // stop main process and wait if necessary
            if ((isMainNeutralProcessWorking && processInfo.Process.StopsMainNeutralProcess)
                || (isMainPlayerProcessWorking && processInfo.Process.StopsMainOpponentProcess && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Enemy))
                || (isMainPlayerProcessWorking && processInfo.Process.StopsMainOwnerProcess && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Player))
                || (isMainEnemyProcessWorking && processInfo.Process.StopsMainOwnerProcess && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Enemy))
                || (isMainEnemyProcessWorking && processInfo.Process.StopsMainOpponentProcess && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Player)))
            {
                // if main process must be stopped
                if (isMainNeutralProcessWorking && processInfo.Process.StopsMainNeutralProcess)
                {
                    ProcessInfo processInfoToStop = new ProcessInfo() { Process = mainNeutralProcess, PendingBehaviours = new List<SO_behaviour>() };
                    processesToStop.Add(processInfoToStop);
                    isMainNeutralProcessWorking = false;
                }

                //if enemy calls to stop opponent process
                if (isMainPlayerProcessWorking && processInfo.Process.StopsMainOpponentProcess && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Enemy))
                {
                    ProcessInfo processInfoToStop = new ProcessInfo() { Process = mainPlayerProcess, PendingBehaviours = new List<SO_behaviour>() };
                    processesToStop.Add(processInfoToStop);
                    isMainPlayerProcessWorking = false;
                }

                //if player calls to stop own process
                if (isMainPlayerProcessWorking && processInfo.Process.StopsMainOwnerProcess && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Player))
                {
                    ProcessInfo processInfoToStop = new ProcessInfo() { Process = mainPlayerProcess, PendingBehaviours = new List<SO_behaviour>() };
                    processesToStop.Add(processInfoToStop);
                    isMainPlayerProcessWorking = false;
                }

                //if enemy calls to stop own process
                if (isMainEnemyProcessWorking && processInfo.Process.StopsMainOwnerProcess && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Enemy))
                {
                    ProcessInfo processInfoToStop = new ProcessInfo() { Process = mainEnemyProcess, PendingBehaviours = new List<SO_behaviour>() };
                    processesToStop.Add(processInfoToStop);
                    isMainEnemyProcessWorking = false;
                }

                //if player calls to stop enemy process
                if (isMainEnemyProcessWorking && processInfo.Process.StopsMainOpponentProcess && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Player))
                {
                    ProcessInfo processInfoToStop = new ProcessInfo() { Process = mainEnemyProcess, PendingBehaviours = new List<SO_behaviour>() };
                    processesToStop.Add(processInfoToStop);
                    isMainEnemyProcessWorking = false;
                }

                foreach (var proces in processesToStop)
                { 
                    // remember all behaviours
                    for (int i = 0; i < proces.Process.Behaviours.Count; i++)
                    {
                        // we have to remember all started behaviours to properly remove processInfoes from the list of current processes
                        proces.PendingBehaviours.Add(proces.Process.Behaviours[i]);
                    }
                }

                // don't do any effects on stop
                foreach (var proces in processesToStop)
                {
                    proces.Process.EffectsOnEnemy.Clear();
                    proces.Process.EffectsOnPlayer.Clear();
                }

                // add to the list of current processes
                foreach (var proces in processesToStop)
                    currentProcesses.Add(proces);

                // wait with the original process (go back to list but at the first position)
                waitingProcesses.AddFirst(processInfo);

                foreach (var proces in processesToStop)
                {
                    // stop all behaviours (must be in a separate loop to prevent finishing before adding to the list of current processes
                    for (int i = 0; i < proces.Process.Behaviours.Count; i++)
                    {
                        // stop the behaviour
                        ExtendedBehaviourController.StopBehaviour(proces.Process.Behaviours[i]);
                    }
                }

            }
            else
            {
                // remember all behaviours
                for (int i = 0; i < processInfo.Process.Behaviours.Count; i++)
                {
                    // we have to remember all started behaviours to properly remove processInfoes from the list of current processes
                    processInfo.PendingBehaviours.Add(processInfo.Process.Behaviours[i]);
                }

                // add to the list of current processes
                currentProcesses.Add(processInfo);

                // play all behaviours (must be in separate loop to prevent finishing before adding to the list of current processes
                for (int i = 0; i < processInfo.Process.Behaviours.Count; i++)
                {
                    // start the behaviour
                    ExtendedBehaviourController.StartBehaviour(processInfo.Process.Behaviours[i], processInfo.OwnerType);
                }
            }

            // apply effects only if this is a "true" process, not a stopping one
            if (processesToStop.Count < 0)
                DeliverEffects(processInfo);
        }

        else
        {
            // remember all behaviours
            for (int i = 0; i < processInfo.Process.Behaviours.Count; i++)
            {
                // we have to remember all started behaviours to properly remove processInfoes from the list of current processes
                processInfo.PendingBehaviours.Add(processInfo.Process.Behaviours[i]);
            }

            // add to the list of current processes
            currentProcesses.Add(processInfo);

            // play all behaviours (must be in separate loop to prevent finishing before adding to the list of current processes
            for (int i = 0; i < processInfo.Process.Behaviours.Count; i++)
            {
                // start the behaviour
                ExtendedBehaviourController.StartBehaviour(processInfo.Process.Behaviours[i], processInfo.OwnerType);
            }

            DeliverEffects(processInfo);
        }


        // if there was no extended behaviours to start then we have to remove a started process from current processes
        // check where the processes are located, if length of processesToStop is 0 it means, that executed process wasn't marked as stop process
        // and is located in processInfo variable
        if(processesToStop.Count > 0)
        {
            foreach(var process in processesToStop)
            {
                if (process.Process.Behaviours.Count <= 0)
                {
                    // remove finished processes and check out if main process started
                    for (int i = currentProcesses.Count - 1; i >= 0; i--)
                    {
                        if (currentProcesses[i].PendingBehaviours.Count < 1)
                        {
                            if (currentProcesses[i].IsMainProcess)
                            {
                                switch (currentProcesses[i].OwnerType)
                                {
                                    case MatchProgressChangingObject.Type.Neutral:
                                        mainNeutralProcess = currentProcesses[i].Process;
                                        isMainNeutralProcessWorking = true;
                                        break;
                                    case MatchProgressChangingObject.Type.Player:
                                        mainPlayerProcess = currentProcesses[i].Process;
                                        isMainPlayerProcessWorking = true;
                                        break;
                                    case MatchProgressChangingObject.Type.Enemy:
                                        mainEnemyProcess = currentProcesses[i].Process;
                                        isMainEnemyProcessWorking = true;
                                        break;

                                }

                            }
                            currentProcesses.RemoveAt(i);
                        }
                    }

                    // raise event if all processes finished and there is no more waiting
                    if (currentProcesses.Count < 1 && waitingProcesses.Count < 1)
                    {
                        EventManager.RaiseEventAllProcessesFinished(isMainNeutralProcessWorking, isMainPlayerProcessWorking, isMainEnemyProcessWorking);
                    }
                    else
                    {
                        // we have to do it eventually
                        TryExecuteNextProcessFromQueue();
                    }
                }
            }
        }

        else if (processInfo.Process.Behaviours.Count <= 0)
        {
            // remove finished processes and check out if main process started
            for (int i = currentProcesses.Count - 1; i >= 0; i--)
            {
                if (currentProcesses[i].PendingBehaviours.Count < 1)
                {
                    if (currentProcesses[i].IsMainProcess)
                    {
                        switch (currentProcesses[i].OwnerType)
                        {
                            case MatchProgressChangingObject.Type.Neutral:
                                mainNeutralProcess = currentProcesses[i].Process;
                                isMainNeutralProcessWorking = true;
                                break;
                            case MatchProgressChangingObject.Type.Player:
                                mainPlayerProcess = currentProcesses[i].Process;
                                isMainPlayerProcessWorking = true;
                                break;
                            case MatchProgressChangingObject.Type.Enemy:
                                mainEnemyProcess = currentProcesses[i].Process;
                                isMainEnemyProcessWorking = true;
                                break;

                        }

                    }
                    currentProcesses.RemoveAt(i);
                }
            }

            // raise event if all processes finished and there is no more waiting
            if (currentProcesses.Count < 1 && waitingProcesses.Count < 1)
            {
                EventManager.RaiseEventAllProcessesFinished(isMainNeutralProcessWorking, isMainPlayerProcessWorking, isMainEnemyProcessWorking);
            }
            else
            {
                // we have to do it eventually
                TryExecuteNextProcessFromQueue();
            }
        }
    }

    /// <summary>
    /// Function that applies effects, depending on who called the process.
    /// </summary>
    /// <param name="processInfo">Process that contains effects on owner and enemy</param>
    private void DeliverEffects(ProcessInfo processInfo)
    {
        // apply effects
        // if OwnerType is assigned and Player (left side) calls the process
        if (!processInfo.OwnerType.Equals(null)
            && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Player))
        {
            ApplyEffectsOn(processInfo.Process.EffectsOnEnemy, enemy);
            ApplyEffectsOn(processInfo.Process.EffectsOnPlayer, player);
        }

        // if OwnerType is assigned and Enemy (right side) calls the process
        else if (!processInfo.OwnerType.Equals(null)
            && processInfo.OwnerType.Equals(MatchProgressChangingObject.Type.Enemy))
        {
            ApplyEffectsOn(processInfo.Process.EffectsOnEnemy, player);
            ApplyEffectsOn(processInfo.Process.EffectsOnPlayer, enemy);
        }

        // if above conditions do not fit
        else
        {
            ApplyEffectsOn(processInfo.Process.EffectsOnEnemy, enemy);
            ApplyEffectsOn(processInfo.Process.EffectsOnPlayer, player);
        }
    }

    /// <summary>
    /// Applies effects on the given target.
    /// Target implements IEffectApplicable thus delivers the functionality through the ApplyEffect() method.
    /// </summary>
    private void ApplyEffectsOn(List<SO_effect> effects, IEffectApplicable target)
    {
        if (target == null) return;

        for (int i = 0; i < effects.Count; i++)
        {
            target.ApplyEffect(effects[i]);
        }
    }


}
