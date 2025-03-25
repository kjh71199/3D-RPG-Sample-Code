using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGiveUpState : MonsterRoamingState
{
    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        navMeshAgent.speed = fsmInfo.Stats.moveSpeed * fsmInfo.GiveUpMoveSpeedModifier;

        base.EnterState(state, data);

        NewRandomDestination(false);
    }
}

