using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect that can be applied on the player or on an enemy.
/// </summary>
public class SO_effect : ScriptableObject
{
    [Header("Effect properties")]
    [Tooltip("A one statistic this effect influence on.")]
    public SO_statistic InfluencedStatistic;

    [Tooltip("Statistic delta. How the effect changes the statistic. May be set to positive or negative value.")]
    public int ChangeValue;

    [Tooltip("How many times the ChangeValue will be applied on the statistic. Use it to decrease/increase a statistic more than once.")]
    public int HowManyTimes = 1;

    [Tooltip("Seconds between each next application of the ChangeValue.")]
    public float IntervalDurationSeconds;

    [Tooltip("Should the statistic return to its previous value?")]
    public bool RevertChangesOnFinish = false;

    [Tooltip("Seconds to wait before reverting applied changes (if RevertChangesOnFinish is set to true).")]
    public float DelayRevertSeconds = 0;

    //[Header("Eliminated effects")]
    //[Tooltip("List of effects eliminated by the effect. It means that when the effect will be applied, all effects from this list will go away if present on the player/enemy.")]
    //public List<SO_effect> EliminatedEffects;
}
