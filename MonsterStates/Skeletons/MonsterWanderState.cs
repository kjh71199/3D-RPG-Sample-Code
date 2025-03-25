using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 배회 상태 컴포넌트
public class MonsterWanderState : MonsterRoamingState
{
    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        navMeshAgent.speed = fsmInfo.Stats.moveSpeed * fsmInfo.WanderMoveSpeedModifier;

        base.EnterState(state, data);

        NewRandomDestination(true);
    }
}
