using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishedTankEffects : ExtendedBehaviour
{
    public bool playerTank = false;

    public GameObject explosion;
    public GameObject fireAndSmoke;

    public GameObject camera;

    private void Start()
    {
        base.Start();

        //if (CMCameraSwitch.Instance == null) return;
        //if (playerTank)
        //    CMCameraSwitch.Instance.playerCamera = camera;
        //else
        //    CMCameraSwitch.Instance.enemyCamera = camera;
    }

    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        camera.SetActive(true);
        if(!playerTank)
            Invoke("TurnOffCamera", 2f);

        explosion.SetActive(true);
        Invoke("showFireAndSmoke", 0.5f);
    }

    void showFireAndSmoke()
    {
        explosion.SetActive(false);
        fireAndSmoke.SetActive(true);
    }

    void TurnOffCamera()
    {
        camera.SetActive(false);
    }

    protected override void StopExtendedBehaviour()
    {
        this.Finish();
    }
}
