using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGolemIdleState : SkeletonGolemState
{
    [SerializeField] protected float checkTime;
    [SerializeField] protected Vector2 checkTimeRange;

    private Health health;

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Health>();
    }

    public override void EnterState(SkeletonGolemFSMController.STATE state, object data = null)
    {
        time = 0f;
        checkTime = Random.Range(checkTimeRange.x, checkTimeRange.y);

        NavigationStop();

        animator.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {
        time = 0f;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;
        if (time < checkTime) return;

        if (controller.GetPlayerDistance() <= fsmInfo.SpecialAttackDistance && fsmInfo.CanSpecialAttack)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.SPECIALATTACK);
            return;
        }
        else if (controller.GetPlayerDistance() <= fsmInfo.AttackDistance)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.ATTACK);
            return;
        }

        if (health.CurrentHp <= health.MaxHp * 0.5f && fsmInfo.CanSummon)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.SUMMON);
            return;
        }

        if (controller.GetPlayerDistance() > fsmInfo.AttackDistance)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.DETECT);
            return;
        }

    }

}
