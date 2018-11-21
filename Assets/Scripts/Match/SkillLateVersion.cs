using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLateVersion : MonoBehaviour
{
    [SerializeField]
    private SO_process fireworksProcess;

    [SerializeField]
    private SO_process setOnFireProcess;

    [SerializeField]
    private SO_process setOnDropProcess;

    [SerializeField]
    private SO_process setOnShootProcess;

    [SerializeField]
    private SO_process setOnSpinTower;

    [SerializeField]
    private SO_process setOnMolotovCocktailProcess;

    [SerializeField]
    private SO_process setOnGrenadeProcess;

    public void UseFireworks()
    {
        StartProcess(fireworksProcess, MatchProgressChangingObject.Type.Neutral);
    }

    public void UseFireByPlayer()
    {
        StartProcess(setOnFireProcess, MatchProgressChangingObject.Type.Player);
    }

    public void UseFireByEnemy()
    {
        StartProcess(setOnFireProcess, MatchProgressChangingObject.Type.Enemy);
    }

    public void UseDropByEnemy()
    {
        StartProcess(setOnDropProcess, MatchProgressChangingObject.Type.Enemy);
    }

    public void UseDropByPlayer()
    {
        StartProcess(setOnDropProcess, MatchProgressChangingObject.Type.Player);
    }

    public void UseMolotovCocktailByEnemy()
    {
        StartProcess(setOnMolotovCocktailProcess, MatchProgressChangingObject.Type.Enemy);
    }

    public void UseMolotovCocktailByPlayer()
    {
        StartProcess(setOnMolotovCocktailProcess, MatchProgressChangingObject.Type.Player);
    }

    public void UseGrenadeByEnemy()
    {
        StartProcess(setOnGrenadeProcess, MatchProgressChangingObject.Type.Enemy);
    }

    public void UseGrenadeByPlayer()
    {
        StartProcess(setOnGrenadeProcess, MatchProgressChangingObject.Type.Player);
    }

    public void UseShootByPlayer()
    {
        StartProcess(setOnShootProcess, MatchProgressChangingObject.Type.Player);
    }

    public void UseSpinTowerByPlayer()
    {
        StartProcess(setOnSpinTower, MatchProgressChangingObject.Type.Player);
    }

    private void StartProcess(SO_process process, MatchProgressChangingObject.Type ownerType)
    {
        if (process == null) return;
        EventManager.RaiseEventProcessStarted(process, ownerType);
    }
}