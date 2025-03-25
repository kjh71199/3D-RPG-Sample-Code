using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterState
{
    private float animationTime;

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        NavigationStop();

        animator.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        animationTime = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);

        if (controller.GetPlayerDistance() > fsmInfo.AttackDistance && animationTime >= 0.9f)
        {
            controller.TransactionToState(MonsterFSMController.STATE.GIVEUP);
            return;
        }
        
        LookAtTarget();
    }

    protected void LookAtTarget()
    {
        Vector3 direction = (controller.Player.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * fsmInfo.Stats.rotateSpeed);
    }
}
