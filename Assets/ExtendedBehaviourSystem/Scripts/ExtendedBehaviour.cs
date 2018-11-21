using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for all ExtendedBehaviours.
/// </summary>
public abstract class ExtendedBehaviour : MonoBehaviour, IBehaviourable
{
    [Tooltip("What type of behaviour it is. Use one of SO_behaviours here.")]
    public SO_behaviour Behaviour;

    // set by the ExtendedBehaviourController to properly count ExtendedBehaviours of each kind that are currently playing
    private List<int> behaviourIds = new List<int>();

    [SerializeField]
    public Button buttonSpawn;

    protected virtual void Awake()
    {
        // maybe we will use this place in the future
    }

    protected virtual void Start ()
    {
        // this leads to registering itself in the system
        EventManager.RaiseEventExtendedBehaviourInstantiated(this);
	}

    /// <summary>
    /// Method called every time some process starts with this kind of behaviour connected.
    /// It calls DoExtendedBehaviour() method inside so please override DoExtendedBehaviour() to inject you own behaviour.
    /// </summary>
    /// <param name="id">Set by the ExtendedBehaviourController to properly count ExtendedBehaviours
    /// <param name="processOwnerType">Who started the process (skill)
    /// of each kind that are currently playing.</param>
    public void DoBehaviour(int id, MatchProgressChangingObject.Type processOwnerType = MatchProgressChangingObject.Type.Player)
    {
        // store id given by the ExtendedBehaviourController
        behaviourIds.Add(id);

        // do some additional behaviour (specified in a derived class)
        // don't if already playing this behaviour
        if (behaviourIds.Count < 2)
        {
            DoExtendedBehaviour(processOwnerType);
        }
    }

    /// <summary>
    /// Used to stop any effects made by the behaviour. Or to finish it earlier.
    /// It's behaviour depends on how the StopExtendedBehaviour() method will be overrided, 
    /// so do not trust this method entirely if you don't know what you are doing.
    /// </summary>
    public void StopBehaviour(int id)
    {
        // store id given by the ExtendedBehaviourController
        behaviourIds.Add(id);

        // do some additional stop behaviour (specified in a derived class)
        StopExtendedBehaviour();
    }

    /// <summary>
    /// Main method of the ExtendedBehaviour. Override to implement the behaviour you want.
    /// </summary>
    protected abstract void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType);

    /// <summary>
    /// Override to implement the stop behaviour you want.
    /// </summary>
    protected abstract void StopExtendedBehaviour();

    /// <summary>
    /// It informs the ExtendedBehaviourController that this ExtendedBehaviour finished playing.
    /// You have to use it when your behaviour ends.
    /// </summary>
    protected void Finish()
    {
        // inform about all behaviourIds at once because we don't want to play one after another anyway
        for (int i = behaviourIds.Count - 1; i >= 0; i--)
        {
            int behaviourId = behaviourIds[i];
            behaviourIds.RemoveAt(i);
            EventManager.RaiseEventExtendedBehaviourFinished(behaviourId);           
        }
    }
}
