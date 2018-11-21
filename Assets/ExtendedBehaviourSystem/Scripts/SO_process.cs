using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds all data of the process. ProcessController knows what to do thanks to that.
/// </summary>
public class SO_process : ScriptableObject
{ 
    [Tooltip("Does the process starts immediately without waiting for other processes to finish?")] 
    public bool IgnoresQueue = false;

    [Tooltip("Does the process pause the main process of the owner?")]
    public bool StopsMainOwnerProcess = false;

    [Tooltip("Does the process pause the main process of the enemy?")]
    public bool StopsMainOpponentProcess = false;

    [Tooltip("Does the process pause the main neutral process?")]
    public bool StopsMainNeutralProcess = false;

    [Tooltip("Behaviours to play.")]
    public List<SO_behaviour> Behaviours;

    [Tooltip("What effects apply on an enemy at the finish of the process.")]
    public List<SO_effect> EffectsOnEnemy;

    [Tooltip("What effects apply on the player at the finish of the process.")]
    public List<SO_effect> EffectsOnPlayer;
}
