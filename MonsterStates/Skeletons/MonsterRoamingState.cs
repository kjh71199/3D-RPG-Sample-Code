using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 몬스터 배회 상태 컴포넌트
public class MonsterRoamingState : MonsterState
{
    protected Transform targetTransform = null;
    public Vector3 targetPosition = Vector3.positiveInfinity;
    public float targetDistance = Mathf.Infinity;

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        animator.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {
        NavigationStop();

        targetTransform = null;
        targetPosition = Vector3.positiveInfinity;
        targetDistance = Mathf.Infinity;
    }

    public override void UpdateState()
    {
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

        if (targetTransform != null)
        {
            targetDistance = Vector3.Distance(transform.position, targetPosition);
            if (targetDistance < 1f)
            {
                controller.TransactionToState(MonsterFSMController.STATE.IDLE);
                return;
            }
        }
    }

    protected virtual void NewRandomDestination(bool retry)
    {
        int index = Random.Range(0, fsmInfo.WanderPoints.Length);
        
        if (fsmInfo.WanderPoints.Length < 1)
        {
            controller.TransactionToState(MonsterFSMController.STATE.IDLE);
            return;
        }

        float distance = Vector3.Distance(fsmInfo.WanderPoints[index].position, transform.position);
        if (distance < fsmInfo.NextPointSelectDistance && retry)
        {
            NewRandomDestination(true);
            return;
        }

        targetTransform = fsmInfo.WanderPoints[index];

        Vector3 randomDirection = Random.insideUnitSphere * fsmInfo.NextPointSelectDistance;
        randomDirection += fsmInfo.WanderPoints[index].position;
        randomDirection.y = 0f;

        targetPosition = randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, fsmInfo.WanderNavCheckRadius, NavMesh.AllAreas))
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = fsmInfo.WanderMoveSpeedModifier;
            navMeshAgent.SetDestination(targetPosition);
        }
    }
}