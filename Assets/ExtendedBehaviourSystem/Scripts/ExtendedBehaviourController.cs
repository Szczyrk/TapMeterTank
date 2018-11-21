using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedBehaviourController : MonoBehaviour
{

    private struct BehaviourWrapper
    {
        public SO_behaviour behaviour;
        public MatchProgressChangingObject.Type type;

        public BehaviourWrapper(SO_behaviour p1, MatchProgressChangingObject.Type p2)
        {
            behaviour = p1;
            type = p2;
        }
    }


    // contains all ExtendedBehaviours (or other IBehaviourables) instantiated in a scene, grouped by a kind of a behaviour
    private Dictionary<SO_behaviour, List<IBehaviourable>> ExtendedBehaviours = new Dictionary<SO_behaviour, List<IBehaviourable>>();
    // <id, count> <-- stores currently playing behaviours grouped by ids
    private Dictionary<int, int> CurrentlyPlayingBehaviours = new Dictionary<int, int>();
    // <id, kind of behaviour> <-- maps id to a kind of a behaviour
    private Dictionary<int, BehaviourWrapper> IdToBehaviours = new Dictionary<int, BehaviourWrapper>();


    private void Awake()
    {
        EventManager.SubscribeToEventExtendedBehaviourInstantiated(OnExtendedBehaviourInstantiated);
        EventManager.SubscribeToEventExtendedBehaviourFinished(OnExtendedBehaviourFinished);
    }

    private void OnDestroy()
    {
        EventManager.UnsubscribeFromEventExtendedBehaviourInstantiated(OnExtendedBehaviourInstantiated);
        EventManager.UnsubscribeFromEventExtendedBehaviourFinished(OnExtendedBehaviourFinished);
    }

 
    private void OnExtendedBehaviourInstantiated(ExtendedBehaviour extendedBehaviour)
    {
        // add the new extended behaviour to the list
        List<IBehaviourable> behaviourables = null;
        if (ExtendedBehaviours.TryGetValue(extendedBehaviour.Behaviour, out behaviourables))
        {
            behaviourables.Add(extendedBehaviour);
        }
        else
        {
            // if list not found then create a new one
            behaviourables = new List<IBehaviourable>();
            behaviourables.Add(extendedBehaviour);
            ExtendedBehaviours.Add(extendedBehaviour.Behaviour, behaviourables);
        }
    }

    private void OnExtendedBehaviourFinished(int behaviourId)
    {
        int playingBehavioursCount = 0;
        if (CurrentlyPlayingBehaviours.TryGetValue(behaviourId, out playingBehavioursCount))
        {
            // decrease count of playing behaviours because one just finished
            playingBehavioursCount--;

            // if zero then all behaviours played
            if (playingBehavioursCount == 0)
            {
                // do not count behaviours for this id anymore
                CurrentlyPlayingBehaviours.Remove(behaviourId);

                // raise event informing which behaviour finished playing
                BehaviourWrapper behaviourPlayed = new BehaviourWrapper(null, MatchProgressChangingObject.Type.Player);
                if (IdToBehaviours.TryGetValue(behaviourId, out behaviourPlayed))
                {
                    IdToBehaviours.Remove(behaviourId);
                    EventManager.RaiseEventAllBehavioursPlayed(behaviourPlayed.behaviour);     
                }
            }
            // else updated value
            else
            {
                CurrentlyPlayingBehaviours[behaviourId] = playingBehavioursCount;
            }
        }
    }

    /// <summary>
    /// Creates a new id.
    /// </summary>
    /// <returns>Newly created id.</returns>
    private int GetNextId()
    {
        int newId = CurrentlyPlayingBehaviours.Count;
        while (CurrentlyPlayingBehaviours.ContainsKey(newId))
        {
            newId++;
        }
        return newId;
    }


    public void StartBehaviour(SO_behaviour behaviour, MatchProgressChangingObject.Type processOwnerType = MatchProgressChangingObject.Type.Player)
    {
        StartOrStopBehaviour(behaviour, true, processOwnerType);
    }

    public void StopBehaviour(SO_behaviour behaviour)
    {
        StartOrStopBehaviour(behaviour, false);
    }

    private void StartOrStopBehaviour(SO_behaviour behaviour, bool doStart, MatchProgressChangingObject.Type processOwnerType = MatchProgressChangingObject.Type.Player)
    {
        // create a new id
        int newId = GetNextId();

        // get extended behaviours to play/stop
        List<IBehaviourable> behaviourables = null;
        if (ExtendedBehaviours.TryGetValue(behaviour, out behaviourables))
        {
            // store all behaviours to play/stop (some of them on the list may be null so we don't want them stored and counted in)
            List <IBehaviourable> extendedBehavioursToPlayOrStop = new List<IBehaviourable>();
            for (int i = 0; i < behaviourables.Count; i++)
            {
                if (behaviourables[i] != null)
                {
                    extendedBehavioursToPlayOrStop.Add(behaviourables[i]);
                }
            }
            behaviourables.Clear();

            // refresh the list
            for (int i = 0; i < extendedBehavioursToPlayOrStop.Count; i++)
            {
                behaviourables.Add(extendedBehavioursToPlayOrStop[i]);
            }

            // store how many extended behaviours will play/stop
            CurrentlyPlayingBehaviours.Add(newId, extendedBehavioursToPlayOrStop.Count);

            // map id to behaviour
            IdToBehaviours.Add(newId, new BehaviourWrapper(behaviour, processOwnerType));

            // play/stop extended behaviours
            for (int i = 0; i < extendedBehavioursToPlayOrStop.Count; i++)
            {
                if (doStart)
                {
                    // play
                    extendedBehavioursToPlayOrStop[i].DoBehaviour(newId, processOwnerType);
                }
                else
                {
                    // stop
                    extendedBehavioursToPlayOrStop[i].StopBehaviour(newId);
                }
            }
            extendedBehavioursToPlayOrStop.Clear();
        }
        // no extended behaviours to play/stop found
        else
        {
            // return a status immediately
            EventManager.RaiseEventAllBehavioursPlayed(behaviour);
        }
    }
}
