using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FallingChest : ExtendedBehaviour
{
    public GameObject chest;
    public GameObject camera;

    public TimeMatch time;
    public MoveUIPanel playerWonPanel;

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        if (time)
            time.Stop();
        Invoke("DropChest", 4f);
    }

    void DropChest()
    {
        camera.SetActive(true);
        chest.SetActive(true);
        chest.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, -5f);
    }

    internal void ChestOpened()
    {
        PlayerStatistic.Instance.GameLevelUp(1);
        playerWonPanel.Show();

    }


    protected override void StopExtendedBehaviour()
    {
        this.Finish();
    }
}
