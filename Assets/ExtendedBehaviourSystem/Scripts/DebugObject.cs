using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugObject : MonoBehaviour
{
    public ExtendedBehaviourController ExtendedBehaviourController;


    public void StartProcess(SO_process process)
    {
        if (process == null) return;
        EventManager.RaiseEventMainProcessStarted(process);
    }

    public void StartBehaviour(SO_behaviour behaviour)
    {
        if (behaviour == null) return;
        ExtendedBehaviourController.StartBehaviour(behaviour);
    }

    public void StartExtendedBehaviour(ExtendedBehaviour extendedBehaviour)
    {
        if (extendedBehaviour == null) return;
        extendedBehaviour.DoBehaviour(-1);
    }
}
