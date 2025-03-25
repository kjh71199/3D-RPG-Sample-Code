using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGolemSpecialAttackState : SkeletonGolemState
{
    protected bool isAttackAnimation;
    protected Vector3 direction;

    public override void EnterState(SkeletonGolemFSMController.STATE state, object data = null)
    {
        animator.SetInteger("State", (int)state);
        direction = (controller.Player.transform.position - transform.position).normalized;
        navMeshAgent.SetDestination(transform.position + (direction * 10f));
        navMeshAgent.speed = fsmInfo.Stats.moveSpeed * fsmInfo.AttackMoveSpeedModifier;
        navMeshAgent.isStopped = false;
        fsmInfo.CanSpecialAttack = false;
        
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        time = 0f;
    }

    public override void ExitState()
    {
        time = 0f;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time > 3.2f)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.IDLE);
            return;
        }
    }

}
