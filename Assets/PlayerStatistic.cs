using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatistic : MonoBehaviour
{

    public static PlayerStatistic Instance;
    static string GAME_LEVEL = "game_level";
    static string SKILL_AMOUNT = "skill_amount";
    static string COINS_AMOUNT = "in_game_coints";
    static string SKILL_SLOT_UNLOCKED = "skill_slot_unlocked";
    static string TANK_LEVEL = "tank_level";
    static string SHOW_TUTORIAL = "show_tutorial";
    public static int amount_Tank = 5;
    int GameLevel = 0;
    bool tutorial = false;

    #region Singleton
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
       
        
       Reset();
        /*
        GameLevel = GetLevelGame();
        GameLevelUp(1);
        Debug.Log("GameLevel = " + GetLevelGame());*/
    }
    #endregion

    public void GameLevelUp(int levelup)
    {
        GameLevel += levelup;

        PlayerPrefs.SetInt(GAME_LEVEL, GameLevel);
        PlayerPrefs.Save();
    }

    public void LevelGameDown(int levelDown)
    {
        GameLevel -= levelDown;

        PlayerPrefs.SetInt(GAME_LEVEL, GameLevel);
        PlayerPrefs.Save();
    }

    public int GetLevelGame()
    {
        return PlayerPrefs.GetInt(GAME_LEVEL, 0);
    }

    public void SetCoinsAmount(int amount)
    {
        PlayerPrefs.SetInt(COINS_AMOUNT, amount);
        PlayerPrefs.Save();
    }

    public int GetCoinsAmount()
    {
        return PlayerPrefs.GetInt(COINS_AMOUNT, 100);
    }

    public void SetTankLevel(Tank.TankType tank, int amount)
    {
        PlayerPrefs.SetInt(TANK_LEVEL + "_" + tank, amount);
        PlayerPrefs.Save();
    }

    public int GetTankLevel(Tank.TankType tank)
    {
        return PlayerPrefs.GetInt(TANK_LEVEL + "_" + tank, 0);
    }

    public void SetSkillAmount(Skill.SkillType skillType, int amount)
    {
        PlayerPrefs.SetInt(SKILL_AMOUNT + "_" + skillType, amount);
        PlayerPrefs.Save();
    }

    public int GetSkillAmount(Skill.SkillType skillType)
    {
        return PlayerPrefs.GetInt(SKILL_AMOUNT + "_" + skillType, 0);
    }

    public void SetSlotUnlocked(int slotID)
    {
        PlayerPrefs.SetInt(SKILL_SLOT_UNLOCKED + "_" + slotID, 1);
    }

    public bool GetSlotUnlocked(int slotID)
    {
        return PlayerPrefs.GetInt(SKILL_SLOT_UNLOCKED + "_" + slotID, 0) == 1;
    }


    public void Tutorial()
        {
        if(tutorial)
            {
            SetDisableShowTutorial(1);
            TutorialStart.Instance.TutorialStartMenu();
            tutorial = false;
            }
        else
            {
            SetDisableShowTutorial(0);
            tutorial = true;
            }
        }


    public void SetDisableShowTutorial(int set)
    {
        PlayerPrefs.SetInt(SHOW_TUTORIAL, set);
    }

    public bool GetShowTutorial()
    {
        return PlayerPrefs.GetInt(SHOW_TUTORIAL, 1) == 1;
    }

    private void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(SKILL_SLOT_UNLOCKED + "_" + 0, 1);
        SetCoinsAmount(1000);
    }
}