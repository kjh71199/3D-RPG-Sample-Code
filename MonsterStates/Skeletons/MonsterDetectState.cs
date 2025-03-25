using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 추적 상태 컴포넌트
public class MonsterDetectState : MonsterState
{
    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        navMeshAgent.speed = fsmInfo.Stats.moveSpeed * fsmInfo.DetectMoveSpeedModifier;

        animator.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {
        animator.speed = 1f;
    }

    public override void UpdateState()
    {
        if (controller.GetPlayerDistance() <= fsmInfo.AttackDistance)
        {
            controller.TransactionToState(MonsterFSMController.STATE.ATTACK);
            return;
        }

        if (controller.GetPlayerDistance() > fsmInfo.DetectDistance)
        {
            controller.TransactionToState(MonsterFSMController.STATE.GIVEUP);
            return;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(controller.Player.transform.position);
    }
}

