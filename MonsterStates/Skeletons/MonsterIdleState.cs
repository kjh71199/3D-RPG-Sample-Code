using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterState
{
    [SerializeField] protected float time;
    [SerializeField] protected float checkTime;
    [SerializeField] protected Vector2 checkTimeRange;

    MonsterRestState restState;

    protected override void Awake()
    {
        base.Awake();
        restState = GetComponent<MonsterRestState>();
    }

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

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

        if (controller.GetPlayerDistance() <= fsmInfo.AttackDistance)
        {
            controller.TransactionToState(MonsterFSMController.STATE.ATTACK);
            return;
        }

        if (controller.GetPlayerDistance() <= fsmInfo.DetectDistance)
        {
            controller.TransactionToState(MonsterFSMController.STATE.DETECT);
            return;
        }

        if (restState != null)
        {
            if (GetFireDistance() <= restState.RestDistance)
            {
                controller.TransactionToState(MonsterFSMController.STATE.REST);
                return;
            }
        }

        if (time > checkTime)
        {
            int selectState = Random.Range(0, 2);

            switch (selectState)
            {
                case 0:
                    time = 0f;
                    checkTime = Random.Range(checkTimeRange.x, checkTimeRange.y);
                    break;
                case 1:
                    controller.TransactionToState(MonsterFSMController.STATE.WANDER);
                    return;
            }
        }
    }

    public float GetFireDistance()
    {
        if (restState.Bonfire != null)
            return Vector3.Distance(transform.position, restState.Bonfire.transform.position);
        else
            return float.MaxValue;
    }
}
