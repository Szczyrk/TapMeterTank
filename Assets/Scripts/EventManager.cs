/*
*	Name: EventManager.cs
* 	Author: Maciej Krzykwa
*	Created: 24.09.2018
*/

using UnityEngine;
using System;


public static partial class EventManager
{
    private static Action EventMatchStarted;
    public static void RaiseEventMatchStarted() { if (EventMatchStarted != null) EventMatchStarted(); }
    public static void SubscribeToEventMatchStarted(Action handlerMethod) { EventMatchStarted += handlerMethod; }
    public static void UnsubscribeFromEventMatchStarted(Action handlerMethod) { EventMatchStarted -= handlerMethod; }

    private static Action EventMatchRestarted;
    public static void RaiseEventMatchRestarted() { if (EventMatchRestarted != null) EventMatchRestarted(); }
    public static void SubscribeToEventMatchRestarted(Action handlerMethod) { EventMatchRestarted += handlerMethod; }
    public static void UnsubscribeFromEventMatchRestarted(Action handlerMethod) { EventMatchRestarted -= handlerMethod; }

    private static Action<bool> EventMatchEnd;
    public static void RaiseEventMatchEnd(bool won) { if (EventMatchEnd != null) EventMatchEnd(won); }
    public static void SubscribeToEventMatchEnd(Action<bool> handlerMethod) { EventMatchEnd += handlerMethod; }
    public static void UnsubscribeFromEventMatchEnd(Action<bool> handlerMethod) { EventMatchEnd -= handlerMethod; }

    private static Action EventMatchPaused;
    public static void RaiseEventMatchPaused() { if (EventMatchPaused != null) EventMatchPaused(); }
    public static void SubscribeToEventMatchPaused(Action handlerMethod) { EventMatchPaused += handlerMethod; }
    public static void UnsubscribeFromEventMatchPaused(Action handlerMethod) { EventMatchPaused -= handlerMethod; }

    private static Action EventMatchResumed;
    public static void RaiseEventMatchResumed() { if (EventMatchResumed != null) EventMatchResumed(); }
    public static void SubscribeToEventMatchResumed(Action handlerMethod) { EventMatchResumed += handlerMethod; }
    public static void UnsubscribeFromEventMatchResumed(Action handlerMethod) { EventMatchResumed -= handlerMethod; }

    private static Action<MatchObject> EventMatchObjectInstantiated;
    public static void RaiseEventMatchObjectInstantiated(MatchObject matchObject) { if (EventMatchObjectInstantiated != null) EventMatchObjectInstantiated(matchObject); }
    public static void SubscribeToEventMatchObjectInstantiated(Action<MatchObject> handlerMethod) { EventMatchObjectInstantiated += handlerMethod; }
    public static void UnsubscribeFromEventMatchObjectInstantiated(Action<MatchObject> handlerMethod) { EventMatchObjectInstantiated -= handlerMethod; }

    private static Action<MatchProgressChangingObject> EventMatchProgressChangingObjectInstantiated;
    public static void RaiseEventMatchProgressChangingObjectInstantiated(MatchProgressChangingObject matchObject) { if (EventMatchProgressChangingObjectInstantiated != null) EventMatchProgressChangingObjectInstantiated(matchObject); }
    public static void SubscribeToEventMatchProgressChangingObjectInstantiated(Action<MatchProgressChangingObject> handlerMethod) { EventMatchProgressChangingObjectInstantiated += handlerMethod; }
    public static void UnsubscribeFromEventMatchProgressChangingObjectInstantiated(Action<MatchProgressChangingObject> handlerMethod) { EventMatchProgressChangingObjectInstantiated -= handlerMethod; }
}

