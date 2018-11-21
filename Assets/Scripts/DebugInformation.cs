using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInformation : MonoBehaviour {

    private void Awake()
    {
#if UNITY_EDITOR
        EventManager.SubscribeToEventMatchEnd((won) => { Log("Match end. Did win? " + (won)); });
        EventManager.SubscribeToEventMatchStarted(() => { Log("Started"); });
        EventManager.SubscribeToEventMatchRestarted(() => { Log("Restarted"); });
        EventManager.SubscribeToEventMatchPaused(() => { Log("Paused"); });
        EventManager.SubscribeToEventMatchResumed(() => { Log("Resumed"); });
#else
        // Turn off self
        gameObject.SetActive(false);
#endif
    }

    private void OnDestroy()
    {
        // Can't unsubscribe annonymous methods
        // But it's just debug object which will be removed in tests and production
    }

    private void Log(string text)
    {
        Debug.Log(text);
    }
}
