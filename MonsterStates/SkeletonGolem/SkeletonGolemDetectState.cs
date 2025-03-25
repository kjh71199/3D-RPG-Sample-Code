using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGolemDetectState : SkeletonGolemState
{
    public override void EnterState(SkeletonGolemFSMController.STATE state, object data = null)
    {
        navMeshAgent.speed = fsmInfo.Stats.moveSpeed * fsmInfo.DetectMoveSpeedModifier;

        animator.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (controller.GetPlayerDistance() <= fsmInfo.AttackDistance && fsmInfo.CanSpecialAttack)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.SPECIALATTACK);
            return;
        }
        else if (controller.GetPlayerDistance() <= fsmInfo.AttackDistance)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.ATTACK);
            return;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(controller.Player.transform.position);
    }
}
