﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFirstLevelController : MatchController
{

    public override void Attack(MatchProgressChangingObject matchProgressChangingObject)
    {
        // Check who's attacking
        if (matchProgressChangingObject.MyType == MatchProgressChangingObject.Type.Player)
        {
            // Calculate power based on player's statistics, enemy's statistics, and this level based pattern/modifier
            float power = (GetFullPower(MatchProgressChangingObject.Type.Player) * 0.5f) - (GetFullPower(MatchProgressChangingObject.Type.Enemy) * 0.2f);
            // Set power to not be lower than 0
            power = power > 0 ? power : 0f;
            // Apply
            ChangeProgressAndMoveMatchObjects(power);
        }
        else if (matchProgressChangingObject.MyType == MatchProgressChangingObject.Type.Enemy)
        {
            // Calculate power based on player's statistics, enemy's statistics, and this level based pattern/modifier
            float power = (GetFullPower(MatchProgressChangingObject.Type.Enemy)) - (GetFullPower(MatchProgressChangingObject.Type.Player) * 0.3f);
            // Set power to not be lower than 0
            power = power > 0 ? power : 0f;
            // Apply
            ChangeProgressAndMoveMatchObjects((-power) * Time.deltaTime); // multiply by deltaTime because this enemy hits in every Update, but it can use coroutine
                                                                          // or something else so it deltaTime doesn't need to always be here
        }
        else if (matchProgressChangingObject.MyType == MatchProgressChangingObject.Type.Neutral)
        {
            // Should do something?
        }
    }

    private float GetFullPower(MatchProgressChangingObject.Type type)
    {
        float totalPower = 0;

        if (type == MatchProgressChangingObject.Type.Player)
        {
            int power = player.Statistics.Find(stat => stat.SO_Statistic.name == "Power").Value;
            int speed = player.Statistics.Find(stat => stat.SO_Statistic.name == "Speed").Value;
            int weight = enemy.Statistics.Find(stat => stat.SO_Statistic.name == "Weight").Value;

            totalPower += power + speed * 0.1f;
            totalPower /= weight;
        }
        else if (type == MatchProgressChangingObject.Type.Enemy)
        {
            int power = enemy.Statistics.Find(stat => stat.SO_Statistic.name == "Power").Value;
            int speed = enemy.Statistics.Find(stat => stat.SO_Statistic.name == "Speed").Value;
            int weight = player.Statistics.Find(stat => stat.SO_Statistic.name == "Weight").Value;

            totalPower += power + speed * 0.1f;
            totalPower /= weight;
        }

        return totalPower > 0 ? totalPower : 0;
    }

    public void ReturnHome()
    {
        LevelLoader.Instance.LoadLevel(0);
    }
}