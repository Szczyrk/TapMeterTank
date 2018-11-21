using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MatchController : MonoBehaviour
{

    // Start object's position between base and target position. Information about current progress
    [Tooltip("Object's position, rotation, and scale between base position, rotation, and scale and target position, rotation, and scale.")]
    [Range(0, 1)]
    [SerializeField]
    protected float startProgress = 0;    // Stores information about start progress value
    public float StartProgress { get { return startProgress; } }
    protected float currentProgress = 0;  // Values [0, 1], stores current tap progress value
    public float CurrentProgress { get { return currentProgress; } }

    // Objects that will be controlled by this controller
    [Tooltip("Tap objects that will be move/rotate/scale forward and backward by this controller")]
    protected List<MatchObject> tapObjects = new List<MatchObject>();

    // Game's states
    protected bool isMatchPaused = true;
    public bool IsMatchPaused { get { return isMatchPaused; } }
    protected bool matchEnded = false;
    public bool MatchEnded { get { return matchEnded; } }

    // Game's player and enemy - there is only one player and one enemy in the match because there will never be need for more (Neutral objects can impact Player/Enemy)
    protected MatchProgressChangingObject player;
    protected MatchProgressChangingObject enemy;


    // METHODS

    #region MonobehaviourMethods
    protected virtual void Awake()
    {
        // When new match object will be instantiated it will store it in a list and affect it
        EventManager.SubscribeToEventMatchObjectInstantiated(AddMatchObjectToTheList);

        // When this kind of object is instantiated, check is it player or enemy and store them in controller. Also raise event they are stored
        EventManager.SubscribeToEventMatchProgressChangingObjectInstantiated(SetControllerInInstantiatedProgressChangingObjectAndStorePlayerOrEnemy);
    }

    protected virtual void OnDestroy()
    {
        EventManager.UnsubscribeFromEventMatchObjectInstantiated(AddMatchObjectToTheList);
        EventManager.UnsubscribeFromEventMatchProgressChangingObjectInstantiated(StorePlayerOrEnemyObject);
    }
    #endregion

    /// <summary>
    /// Creates match object that can change progress.
    /// </summary>
    /// <param name="matchObject"></param>
    public virtual void CreateChangingProgressMatchObject(GameObject matchObject, MatchProgressChangingObject.Type type)
    {
        if (matchObject == null) return;

        // Check has prefab the MatchObject component
        if (matchObject.GetComponent<MatchProgressChangingObject>())
        {
            var newObject = CreateGameObject(matchObject);
            if (newObject)
            {
                // Get MatchObject component from instantiated object
                var newMatchObject = newObject.GetComponent<MatchProgressChangingObject>();
                if (newMatchObject)
                {
                    // Store this controller
                    newMatchObject.Controller = this;
                }
                else
                {
                    Debug.LogWarning("MatchProgressChangingObject component not found on instantiated " + matchObject.name);
                }
            }
            else
            {
                Debug.LogWarning("Couldn't instantiate " + matchObject.name);
            }
        }
        else
        {
            Debug.LogWarning("MatchProgressChangingObject component not found on " + matchObject.name);
        }
    }

    /// <summary>
    /// Creates match object.
    /// </summary>
    /// <param name="matchObject"></param>
    public virtual void CreateMatchObject(GameObject matchObject)
    {
        if (matchObject == null) return;

        // Check has prefab the MatchObject component
        if (matchObject.GetComponent<MatchObject>())
        {
            var newObject = CreateGameObject(matchObject);
            if (!newObject)
            {
                Debug.LogWarning("Couldn't instantiate " + matchObject.name);
            }
        }
        else
        {
            Debug.LogWarning("MatchObject component not found on " + matchObject.name);
        }
    }

    /// <summary>
    /// Adds match object to the list of objects that will be affected by this match controller.
    /// </summary>
    /// <param name="matchObject"></param>
    protected virtual void AddMatchObjectToTheList(MatchObject matchObject)
    {
        if (matchObject == null) return;

        // Add to the list, so it will be affected by this match controller
        tapObjects.Add(matchObject);
    }

    /// <summary>
    /// Starts match.
    /// </summary>
    public virtual void StartMatch()
    {
        SetToStartValues();

        // Raise event
        EventManager.RaiseEventMatchStarted();
    }

    /// <summary>
    /// Restarts match.
    /// </summary>
    public virtual void RestartMatch()
    {
        SetToStartValues();

        // Raise event
        EventManager.RaiseEventMatchRestarted();
    }

    /// <summary>
    /// Pauses match.
    /// </summary>
    public virtual void PauseMatch()
    {
        SetMatchIsPaused(true);

        EventManager.RaiseEventMatchPaused();
    }

    /// <summary>
    /// Resumes match.
    /// </summary>
    public virtual void ResumeMatch()
    {
        SetMatchIsPaused(false);

        EventManager.RaiseEventMatchResumed();
    }

    /// <summary>
    /// Adds value to current progress [0,1]. Move tap objects that are connected with this controller. Works only if match is not paused and didn't end.
    /// </summary>
    /// <param name="value"></param>
    protected virtual void ChangeProgressAndMoveMatchObjects(float value)
    {
        // Check is it worth to do anything
        if (value != 0.0f)
        {
            // Check is game not paused and didn't end
            if (!isMatchPaused)
            {
                if (!matchEnded)
                {
                    // Add to progress
                    ChangeProgress(value);

                    // Move connected tap objects
                    MoveTapObjects();
                }
            }
        }
    }

    /// <summary>
    /// Sets progress value
    /// </summary>
    /// <param name="matchProgressChangingObject"></param>
    /// <param name="progressValue"></param>
    public virtual void SetProgress(MatchProgressChangingObject matchProgressChangingObject, float progressValue)
    {
        if (matchProgressChangingObject == null) return;

        // Set progress value to exactly progressValue
        ChangeProgressAndMoveMatchObjects(progressValue - currentProgress);
    }

    /// <summary>
    /// Calculates power of player/enemy hit taking in a count their statistics, and change current progress according to that.
    /// </summary>
    /// <param name="byPlayer"></param>
    public abstract void Attack(MatchProgressChangingObject matchProgressChangingObject);

    /// <summary>
    /// Creates a given object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected virtual GameObject CreateGameObject(GameObject obj)
    {
        // This only instantiate the object but it may use pooling in the future
        return Instantiate(obj);
    }

    protected virtual void SetControllerInInstantiatedProgressChangingObjectAndStorePlayerOrEnemy(MatchProgressChangingObject matchProgressChangingObject)
    {
        if (matchProgressChangingObject == null) return;

        matchProgressChangingObject.Controller = this;

        StorePlayerOrEnemyObject(matchProgressChangingObject);
    }

    /// <summary>
    /// If player or enemy is passed via argument they are stored in controller's variables. If neutral object is passed nothing happens.
    /// </summary>
    /// <param name="matchProgressChangingObject"></param>
    protected virtual void StorePlayerOrEnemyObject(MatchProgressChangingObject matchProgressChangingObject)
    {
        if (matchProgressChangingObject == null) return;

        if (matchProgressChangingObject.MyType == MatchProgressChangingObject.Type.Player)
        {
            player = matchProgressChangingObject;
            EventManager.RaiseEventEffectApplicablePlayerInstantiated(matchProgressChangingObject);
        }
        else if (matchProgressChangingObject.MyType == MatchProgressChangingObject.Type.Enemy)
        {
            enemy = matchProgressChangingObject;
            EventManager.RaiseEventEffectApplicableEnemyInstantiated(matchProgressChangingObject);
        }
    }

    /// <summary>
    /// Sets values that change during match to start values.
    /// </summary>
    protected virtual void SetToStartValues()
    {
        // Reset
        SetMatchIsPaused(false);
        matchEnded = false;

        // Start
        currentProgress = StartProgress;

        // Reset and activate tap objects
        ActivateAndSetToCurrentProgressTapObjects();
    }

    /// <summary>
    /// Sets match is paused. It disable possibility to tap and stops moving tap object's.
    /// </summary>
    protected virtual void SetMatchIsPaused(bool status)
    {
        isMatchPaused = status;
    }

    /// <summary>
    /// Ends and pause match.
    /// </summary>
    protected virtual void EndMatch()
    {
        // Set game end
        matchEnded = true;

        // Set game is paused
        SetMatchIsPaused(true);
    }

    /// <summary>
    /// Wins and ends match.
    /// </summary>
    protected virtual void WinMatch()
    {
        EndMatch();

        // Won
        EventManager.RaiseEventMatchEnd(true);
    }

    /// <summary>
    /// Loses and ends match.
    /// </summary>
    protected virtual void LoseMatch()
    {
        // End game
        EndMatch();

        // Lost
        EventManager.RaiseEventMatchEnd(false);
    }

    /// <summary>
    /// Checks is progress equal or bigger than one which means game is won. Runs Win method.
    /// </summary>
    protected virtual void CheckIsMatchWon()
    {
        if (currentProgress >= 1)
        {
            // WON
            WinMatch();
        }
    }

    /// <summary>
    /// Checks is progress equal or lower than zero which means game is lost. Runs Lose method.
    /// </summary>
    protected virtual void CheckIsMatchLost()
    {
        if (currentProgress <= 0)
        {
            // LOSE
            LoseMatch();
        }
    }

    /// <summary>
    /// Checks is progress equal or lower than zero, or equal or bigger than one. Calls win or lose methods.
    /// </summary>
    protected virtual void CheckIsMatchWonOrLost()
    {
        CheckIsMatchWon();
        CheckIsMatchLost();
    }

    /// <summary>
    /// Change player's progress
    /// </summary>
    /// <param name="value"></param>
    protected virtual void ChangeProgress(float value)
    {
        if (!matchEnded)
        {
            currentProgress += value;
            // Guards
            if (currentProgress >= 1)
            {
                currentProgress = 1;
            }
            else if (currentProgress < 0)
            {
                currentProgress = 0;
            }

            // Check game status
            CheckIsMatchWonOrLost();
        }
    }

    /// <summary>
    /// Moves all connected tap objects.
    /// </summary>
    protected virtual void MoveTapObjects()
    {
        // Remove null objects
        tapObjects.RemoveAll((tapObj) => { return tapObj == null ? true : false; });

        // Move objects
        foreach (var tapObject in tapObjects)
        {
            // Check because we're super-safe (we want this game to work well on WebGL)
            if (tapObjects != null)
            {
                tapObject.MakeStep(CurrentProgress);
            }
        }
    }

    /// <summary>
    /// Activates and sets to current progress all stored tap objects.
    /// </summary>
    protected virtual void ActivateAndSetToCurrentProgressTapObjects()
    {
        // Remove null objects
        tapObjects.RemoveAll((tapObj) => { return tapObj == null ? true : false; });

        // Move objects
        foreach (var tapObject in tapObjects)
        {
            // Check because we're super-safe (we want this game to work well on WebGL)
            if (tapObjects != null)
            {
                tapObject.ActivateAndSetToCurrentProgress(this);
            }
        }
    }
}
