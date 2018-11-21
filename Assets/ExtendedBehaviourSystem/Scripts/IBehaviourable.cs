using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourable
{
    void DoBehaviour(int id, MatchProgressChangingObject.Type processOwnerType = MatchProgressChangingObject.Type.Player);
    void StopBehaviour(int id);
}
