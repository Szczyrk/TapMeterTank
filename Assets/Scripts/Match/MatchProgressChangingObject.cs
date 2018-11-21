using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchProgressChangingObject : ExtendedBehaviour, IEffectApplicable
{
    [System.Serializable]
    public enum Type { Neutral, Enemy, Player };

    [System.Serializable]
    public class Statistic
    {
        [SerializeField]
        private SO_statistic SO_statistic;
        public SO_statistic SO_Statistic { get { return SO_statistic; } set { SO_statistic = value; }  }

        [SerializeField]
        private int value;
        public int Value { get { return value; } set { this.value = value; } }
    }

    private class AppliedEffect
    {
        public SO_effect EffectInfo;

        private int summarizedChange;                 // how the statistic was influenced by the effect
        private int howManyTimesLeft;                   // times the statistic value will be to change yet
        private float timeLeftToNextChange;             // time counter
        private float timeLeftToRevertChanges;          // time counter measuring time we wait to revert changes made by the effect

        public AppliedEffect(SO_effect effect)
        {
            EffectInfo = effect;

            summarizedChange = 0;
            howManyTimesLeft = EffectInfo.HowManyTimes;
            timeLeftToNextChange = EffectInfo.IntervalDurationSeconds;
            timeLeftToRevertChanges = EffectInfo.DelayRevertSeconds;
        }

        /// <summary>
        /// Custom update. Calculates time left to the next value change and changes it if there is no time left.
        /// Also reverts a statistic's summarized value change after the last iteration of the effect if the effect says to do that.
        /// </summary>
        public void Update(float deltaTime, MatchProgressChangingObject matchProgressChangingObject)
        {
            // calculate time left and apply change ...
            if (howManyTimesLeft > 0)
            {
                // decrease time left
                timeLeftToNextChange -= deltaTime;

                // apply change
                while (timeLeftToNextChange <= 0 && howManyTimesLeft > 0)
                {
                    // reset timeLeftToChange taking into account that it could be a negative value (means some time of the next iteration already left)
                    timeLeftToNextChange = (EffectInfo.IntervalDurationSeconds + timeLeftToNextChange);

                    // apply changes
                    ApplyChangesOnce(matchProgressChangingObject);

                    // special case to not loose one frame time if the DelayRevertSeconds was set to 0
                    if (howManyTimesLeft <= 0)
                    {
                        if (timeLeftToRevertChanges <= 0)
                        {
                            if (EffectInfo.RevertChangesOnFinish)
                            {
                                RevertChanges(matchProgressChangingObject);
                            }
                        }
                    }
                }   
            }
            // ... or do a delayed revert changes if no apply-times left
            else
            {
                // decrease time left to revert changes
                timeLeftToRevertChanges -= deltaTime;

                // apply revert changes
                if (timeLeftToRevertChanges <= 0)
                {
                    if (EffectInfo.RevertChangesOnFinish)
                    {
                        RevertChanges(matchProgressChangingObject);
                    }
                }    
            }
        }

        /// <summary>
        /// Reverts all changes made by the effect. It is used in the Update() method of the AppliedEffect class.
        /// </summary>
        private void RevertChanges(MatchProgressChangingObject matchProgressChangingObject)
        {
            matchProgressChangingObject.ChangeStatisticValue(-summarizedChange, EffectInfo.InfluencedStatistic);
            timeLeftToNextChange = 0;
            timeLeftToRevertChanges = 0;
        }

        /// <summary>
        /// Applies changes on the effect and stores the information that it was once applied. It is used in the Update() method of the AppliedEffect class.
        /// </summary>
        public void ApplyChangesOnce(MatchProgressChangingObject matchProgressChangingObject)
        {
            matchProgressChangingObject.ChangeStatisticValue(EffectInfo.ChangeValue, EffectInfo.InfluencedStatistic);
            howManyTimesLeft--;
            summarizedChange += EffectInfo.ChangeValue;
            EventManager.RaiseEventEffectApplied(EffectInfo);
        }

        /// <summary>
        /// Tells if an effect is ready to be removed. It basically means that all counters are 0 or less.
        /// </summary>
        public bool IsReadyToRemove()
        {
            if (howManyTimesLeft <= 0 && timeLeftToNextChange <= 0 && timeLeftToRevertChanges <= 0)
            {
                return true;
            }

            return false;
        }
    }

    // Match controller
    [HideInInspector]
    public MatchController Controller;

    [Tooltip("Which type is it")]
    [SerializeField]
    protected Type myType;
    public virtual Type MyType { get { return myType; } }

    // PARAMETERS
    [Tooltip("Object's statistics")]
    [SerializeField]
    protected List<Statistic> statistics;
    public List<Statistic> Statistics { get { return statistics; } set { statistics = value; } }

    // Is paused
    protected bool isPaused = true;

    private List<AppliedEffect> appliedEffects = new List<AppliedEffect>();

    #region MonobehavioursMethods
    protected override void Start()
    {
        base.Start();
        EventManager.RaiseEventMatchProgressChangingObjectInstantiated(this);
    }

    protected virtual void Update()
    {
        // update effects
        if (appliedEffects.Count > 0)
        {
            if (!isPaused)
            {
                for (int i = 0; i < appliedEffects.Count; i++)
                {
                    appliedEffects[i].Update(Time.deltaTime, this);
                }
            }

            // remove finished effects
            for (int i = appliedEffects.Count - 1; i >= 0; i--)
            {
                if (appliedEffects[i].IsReadyToRemove())
                {
                    appliedEffects.RemoveAt(i);
                }
            }
        }
    }
    #endregion

    public void ApplyEffect(SO_effect effect)
    {
        AppliedEffect newEffect = new AppliedEffect(effect);

        // remember the effect
        appliedEffects.Add(newEffect);

        // perform the first value change immediately
        newEffect.ApplyChangesOnce(this);
    }

    /// <summary>
    /// Changes controller's progress with calculated power [0, 1].
    /// </summary>
    protected virtual void Attack(MatchProgressChangingObject MatchProgressChangingObject)
    {
        // Check
        if (Controller == null)
        {
            Debug.LogError("Controller is not set!");
            return;
        }

        // Do hit
        Controller.Attack(MatchProgressChangingObject);
    }

    /// <summary>
    /// Changes the statistic by the given value.
    /// </summary>
    protected void ChangeStatisticValue(int changeValue, SO_statistic statisticToChange)
    {
        for (int i = 0; i < Statistics.Count; i++)
        {
            if (Statistics[i].SO_Statistic == statisticToChange)
            {
                Statistics[i].Value += changeValue;
            }
        }
    }

    #region ExtendedBehaviour
    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        isPaused = false;
        this.Finish();
    }

    protected override void StopExtendedBehaviour()
    {
        isPaused = true;
        this.Finish();
    }
    #endregion
}
