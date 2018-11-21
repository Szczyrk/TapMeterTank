using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public int level = 0;
    public TankType tankType;

    [SerializeField]
    public List<TankLevel> TankLevels;

    public enum TankType { BT_5, KV_1, Panther, PzKpfw_IV, T34_85 }

    bool change;

    [Tooltip("Object's statistics")]
    [SerializeField]
    private List<MatchProgressChangingObject.Statistic> statistics = new List<MatchProgressChangingObject.Statistic>();
    public List<MatchProgressChangingObject.Statistic> Statistics { get { return statistics; } set { statistics = value; } }

    public int[] equalizationLevel = new int[3];

    void Start()
    {
        level = PlayerStatistic.Instance.GetTankLevel(tankType);
        setStatistics();

        if (change)
        {
            chanageStatistic();
            change = false;
        }
    }

    public void LevelUp()
    {
        Debug.Log(TankLevels[level].costToLvlUp);
        CoinsPanel.Instance.CoinsSpent(TankLevels[level].costToLvlUp);
        level++;
        PlayerStatistic.Instance.SetTankLevel(tankType, level);
        setStatistics();
        StatisticsPresenter.Instance.SetStatistics(this);
    }

    public void LevelDown(int levelDown)
    {
        level -= levelDown;
        PlayerStatistic.Instance.SetTankLevel(tankType, level);
        change = true;
    }

    public int GetLevel()
    {
        return level;
    }

    void chanageStatistic()
    {
        for(int i=0; i<Statistics.Count; i++)
        {
            Statistics[i].Value += level*equalizationLevel[i];
        }
    }

    void setStatistics()
    {
        Statistics.Find(stat => stat.SO_Statistic.name == "Power").Value = TankLevels[level].power;
        Statistics.Find(stat => stat.SO_Statistic.name == "Speed").Value = TankLevels[level].speed;
        Statistics.Find(stat => stat.SO_Statistic.name == "Weight").Value = TankLevels[level].weight;
    }
}

[System.Serializable]
public class TankLevel
{
    public int costToLvlUp;
    public int power;
    public int speed;
    public int weight;
}
