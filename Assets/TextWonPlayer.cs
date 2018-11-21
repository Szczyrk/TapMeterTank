using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextWonPlayer : ExtendedBehaviour
{
    public GameObject textWon;
    protected override void DoExtendedBehaviour(MatchProgressChangingObject.Type processOwnerType)
    {
        textWon.SetActive(true);
    }

    protected override void StopExtendedBehaviour()
    {
        this.Finish();
    }
}

