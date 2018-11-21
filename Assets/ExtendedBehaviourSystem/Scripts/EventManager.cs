using UnityEngine;
using System;


public static partial class EventManager
{
    private static Action<ExtendedBehaviour> EventExtendedBehaviourInstantiated;
    public static void RaiseEventExtendedBehaviourInstantiated(ExtendedBehaviour extendedBehaviour) { if (EventExtendedBehaviourInstantiated != null) EventExtendedBehaviourInstantiated(extendedBehaviour); }
    public static void SubscribeToEventExtendedBehaviourInstantiated(Action<ExtendedBehaviour> handlerMethod) { EventExtendedBehaviourInstantiated += handlerMethod; }
    public static void UnsubscribeFromEventExtendedBehaviourInstantiated(Action<ExtendedBehaviour> handlerMethod) { EventExtendedBehaviourInstantiated -= handlerMethod; }

    private static Action<int> EventExtendedBehaviourFinished;
    public static void RaiseEventExtendedBehaviourFinished(int behaviourId) { if (EventExtendedBehaviourFinished != null) EventExtendedBehaviourFinished(behaviourId); }
    public static void SubscribeToEventExtendedBehaviourFinished(Action<int> handlerMethod) { EventExtendedBehaviourFinished += handlerMethod; }
    public static void UnsubscribeFromEventExtendedBehaviourFinished(Action<int> handlerMethod) { EventExtendedBehaviourFinished -= handlerMethod; }

    private static Action<SO_process, MatchProgressChangingObject.Type> EventProcessStarted;
    public static void RaiseEventProcessStarted(SO_process process, MatchProgressChangingObject.Type  ownerType = MatchProgressChangingObject.Type.Neutral) { if (EventProcessStarted != null) EventProcessStarted(process, ownerType); }
    public static void SubscribeToEventProcessStarted(Action<SO_process, MatchProgressChangingObject.Type> handlerMethod) { EventProcessStarted += handlerMethod; }
    public static void UnsubscribeFromEventProcessStarted(Action<SO_process, MatchProgressChangingObject.Type> handlerMethod) { EventProcessStarted -= handlerMethod; }

    private static Action<SO_process, MatchProgressChangingObject.Type> EventMainProcessStarted;
    public static void RaiseEventMainProcessStarted(SO_process process, MatchProgressChangingObject.Type ownerType = MatchProgressChangingObject.Type.Neutral) { if (EventMainProcessStarted != null) EventMainProcessStarted(process, ownerType); }
    public static void SubscribeToEventMainProcessStarted(Action<SO_process, MatchProgressChangingObject.Type> handlerMethod) { EventMainProcessStarted += handlerMethod; }
    public static void UnsubscribeFromEventMainProcessStarted(Action<SO_process, MatchProgressChangingObject.Type> handlerMethod) { EventMainProcessStarted -= handlerMethod; }

    private static Action<bool, bool, bool> EventAllProcessesFinished;
    public static void RaiseEventAllProcessesFinished(bool isMainProcessWorking, bool isMainPlayerProcessWorking, bool isMainEnemyProcessWorking) { if (EventAllProcessesFinished != null) EventAllProcessesFinished(isMainProcessWorking, isMainPlayerProcessWorking, isMainEnemyProcessWorking); }
    public static void SubscribeToEventAllProcessesFinished(Action<bool, bool, bool> handlerMethod) { EventAllProcessesFinished += handlerMethod; }
    public static void UnsubscribeFromEventAllProcessesFinished(Action<bool, bool, bool> handlerMethod) { EventAllProcessesFinished -= handlerMethod; }

    private static Action<SO_behaviour> EventAllBehavioursPlayed;
    public static void RaiseEventAllBehavioursPlayed(SO_behaviour behaviour) { if (EventAllBehavioursPlayed != null) EventAllBehavioursPlayed(behaviour); }
    public static void SubscribeToEventAllBehavioursPlayed(Action<SO_behaviour> handlerMethod) { EventAllBehavioursPlayed += handlerMethod; }
    public static void UnsubscribeFromEventAllBehavioursPlayed(Action<SO_behaviour> handlerMethod) { EventAllBehavioursPlayed -= handlerMethod; }

    private static Action<IEffectApplicable> EventEffectApplicablePlayerInstantiated;
    public static void RaiseEventEffectApplicablePlayerInstantiated(IEffectApplicable playerObject) { if (EventEffectApplicablePlayerInstantiated != null) EventEffectApplicablePlayerInstantiated(playerObject); }
    public static void SubscribeToEventEffectApplicablePlayerInstantiated(Action<IEffectApplicable> handlerMethod) { EventEffectApplicablePlayerInstantiated += handlerMethod; }
    public static void UnsubscribeFromEventEffectApplicablePlayerInstantiated(Action<IEffectApplicable> handlerMethod) { EventEffectApplicablePlayerInstantiated -= handlerMethod; }

    private static Action<IEffectApplicable> EventEffectApplicableEnemyInstantiated;
    public static void RaiseEventEffectApplicableEnemyInstantiated(IEffectApplicable enemyObject) { if (EventEffectApplicableEnemyInstantiated != null) EventEffectApplicableEnemyInstantiated(enemyObject); }
    public static void SubscribeToEventEffectApplicableEnemyInstantiated(Action<IEffectApplicable> handlerMethod) { EventEffectApplicableEnemyInstantiated += handlerMethod; }
    public static void UnsubscribeFromEventEffectApplicableEnemyInstantiated(Action<IEffectApplicable> handlerMethod) { EventEffectApplicableEnemyInstantiated -= handlerMethod; }


    private static Action<SO_effect> EventEffectApplied;
    public static void RaiseEventEffectApplied(SO_effect appliedEffect) { if (EventEffectApplied != null) EventEffectApplied(appliedEffect); }
    public static void SubscribeToEventEffectApplied(Action<SO_effect> handlerMethod) { EventEffectApplied += handlerMethod; }
    public static void UnsubscribeFromEventEffectApplied(Action<SO_effect> handlerMethod) { EventEffectApplied -= handlerMethod; }
}

