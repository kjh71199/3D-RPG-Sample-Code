using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWanderState : MonsterRoamingState
{
    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        navMeshAgent.speed = fsmInfo.Stats.moveSpeed * fsmInfo.WanderMoveSpeedModifier;

        base.EnterState(state, data);

        NewRandomDestination(true);
    }
}
