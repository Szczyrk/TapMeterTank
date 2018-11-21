using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorsFirstLevelController : MatchController
{
    public override void Attack(MatchProgressChangingObject matchProgressChangingObject)
    {
        // Check who's attacking
        if (matchProgressChangingObject.MyType == MatchProgressChangingObject.Type.Player)
        {
            // Calculate power based on player's statistics, enemy's statistics, and this level based pattern/modifier
            float power = (GetFullPower(MatchProgressChangingObject.Type.Player)*0.5f) - (GetFullPower(MatchProgressChangingObject.Type.Enemy) * 0.2f);
            // Set power to not be lower than 0
            power = power > 0 ? power : 0f;
            // Apply
            ChangeProgressAndMoveMatchObjects(power);
        }
        else if (matchProgressChangingObject.MyType == MatchProgressChangingObject.Type.Enemy)
        {
            // Calculate power based on player's statistics, enemy's statistics, and this level based pattern/modifier
            var power = (GetFullPower(MatchProgressChangingObject.Type.Enemy)) - (GetFullPower(MatchProgressChangingObject.Type.Player) * 0.3f);
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

    private int GetFullPower(MatchProgressChangingObject.Type type)
    {
        int power = 0;
        if (type == MatchProgressChangingObject.Type.Player)
        {
            foreach (var stat in player.Statistics)
            {
                    power += stat.Value;
            }
        }
        else if (type == MatchProgressChangingObject.Type.Enemy)
        {
            foreach (var stat in enemy.Statistics)
            {
                    power += stat.Value;
            }
        }

        return power > 0 ? power : 0;
    }
}
