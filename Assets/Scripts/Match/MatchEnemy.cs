using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchEnemy : MatchProgressChangingObject
{
    // METHODS
    public int time;
    bool startCoroutine = true;
    #region UnityMethods
    protected override void Update()
    {
        base.Update();

        if (!isPaused)
        {
            // Decrease progress
            Attack(this);
        }
    }
    #endregion

    [SerializeField]
    public List<MenagerLevelsPlayer.SkillEnemy> skillEnemies;


    private void LateUpdate()
    {
        if (startCoroutine)
        {
            startCoroutine = false;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        useSkillByEnemy();
    }
    void useSkillByEnemy()
    {
        if (skillEnemies.Count != 0)
        {
            int skillUse = Random.Range(0, skillEnemies.Count);
            if(skillEnemies[skillUse].Process)
                if (skillEnemies[skillUse].Value > 0)
                {
                    EventManager.RaiseEventProcessStarted(skillEnemies[skillUse].Process, MatchProgressChangingObject.Type.Enemy);
                    skillEnemies[skillUse].Value--;
                    startCoroutine = true;
                }

        }
    }

}
