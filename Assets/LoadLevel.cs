using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    [SerializeField]
    MatchEnemy matchEnemy;
    MenagerLevelsPlayer menager;
    int level;
    GameObject myTank;
   public SO_behaviour behaviour;


    void SwapPrefabs(GameObject oldGameObject)
    {
        // Determine the rotation and position values of the old game object.
        // Replace rotation with Quaternion.identity if you do not wish to keep rotation.
        Vector3 position = oldGameObject.transform.position;
        Quaternion rotation = Quaternion.Euler(oldGameObject.transform.rotation.x, -180, oldGameObject.transform.rotation.z);
        // Instantiate the new game object at the old game objects position and rotation.
        GameObject newGameObject = Instantiate(myTank, position, rotation);

        // If the old game object has a valid parent transform,
        // (You can remove this entire if statement if you do not wish to ensure your
        // new game object does not keep the parent of the old game object.
        if (oldGameObject.transform.parent != null)
        {
            // Set the new game object parent as the old game objects parent.
            newGameObject.transform.SetParent(oldGameObject.transform.parent);
        }
        newGameObject.tag = "Enemy";
        newGameObject.GetComponent<GameFinishedTankEffects>().Behaviour = behaviour;
       // newGameObject.transform.GetChild(0).gameObject.GetComponent<TankDestroyedAnimation>().enabled = false;
        newGameObject.transform.GetChild(0).GetComponent<TankDestroyedAnimation>().Behaviour = behaviour;
        newGameObject.transform.GetChild(0).GetComponent<TankDestroyedAnimation>().bodyTargetHeight = 0f;
        // Destroy the old game object, immediately, so it takes effect in the editor.
        DestroyImmediate(oldGameObject);
        DestroyImmediate(myTank);
    }


    void Start () {

        menager = MenagerLevelsPlayer.Instance;
        level = (PlayerStatistic.Instance.GetLevelGame() - 1);
        if (level < 0)
            return;
       
        ChangeStatisticEnemy();
        if(menager.Levels[level].skillEnemies.Count != 0)
            SkillsEnemy();
        myTank = Instantiate(Resources.Load("Prefabs/Tanks/" + menager.Levels[level].TankType)) as GameObject;
        myTank.tag = "ChooseEnemy";
        DontDestroyOnLoad(myTank);
        myTank = GameObject.FindGameObjectWithTag("ChooseEnemy");
        SwapPrefabs(GameObject.FindGameObjectWithTag("Enemy").transform.gameObject);
    }

    void ChangeStatisticEnemy()
    {
        matchEnemy.Statistics.RemoveAll(Statistic => Statistic.SO_Statistic);
        
         List<MatchProgressChangingObject.Statistic>  statisticsEnemy = menager.Levels[level].statisticsEnemy;
        for (int i = 0; i < statisticsEnemy.Count; i++)
        {
            matchEnemy.Statistics.Add(statisticsEnemy[i]);
        }
    }

    void SkillsEnemy()
    {
        matchEnemy.time = menager.Levels[level].timeWaitSkillbyEnemy;
        matchEnemy.skillEnemies.RemoveAll(skillEnemies => skillEnemies.Process);

        List<MenagerLevelsPlayer.SkillEnemy> skillEnemies1 = menager.Levels[level].skillEnemies;
        for (int i = 0; i < skillEnemies1.Count; i++)
        {
            matchEnemy.skillEnemies.Add(skillEnemies1[i]);
        }
    }
}
