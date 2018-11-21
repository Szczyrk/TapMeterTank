using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenagerLevelsPlayer : MonoBehaviour
{

    static public MenagerLevelsPlayer Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    [System.Serializable]
    public class Level
    {
   

        [SerializeField]
        public List<MatchProgressChangingObject.Statistic> statisticsEnemy = new List<MatchProgressChangingObject.Statistic>();
        [SerializeField]
        public List<SkillEnemy> skillEnemies = new List<SkillEnemy>();
        public int scene;
        public int timeWaitSkillbyEnemy;
        public Tank.TankType TankType;
    }

    [System.Serializable]
    public class SkillEnemy
    {
        [SerializeField]
        private SO_process process;
        public SO_process Process { get { return process; } set { process = value; } }

        [SerializeField]
        private int value;
        public int Value { get { return value; } set { this.value = value; } }

    }

    [SerializeField]
    protected List<Level> levels;
    public List<Level> Levels { get { return levels; } set { levels = value; } }
}
