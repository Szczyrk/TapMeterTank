using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSkillsController : MonoBehaviour
{
    public void StartProcess(SO_process process)
    {
        if (process == null) return;
        EventManager.RaiseEventProcessStarted(process);
    }
}
