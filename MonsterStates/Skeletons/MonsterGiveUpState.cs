using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 도망 상태 컴포넌트
public class MonsterGiveUpState : MonsterRoamingState
{
    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        navMeshAgent.speed = fsmInfo.Stats.moveSpeed * fsmInfo.GiveUpMoveSpeedModifier;

        base.EnterState(state, data);

        NewRandomDestination(false);
    }
}

