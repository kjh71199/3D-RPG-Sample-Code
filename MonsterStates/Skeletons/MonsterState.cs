using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 몬스터 FSM 추상 클래스
public abstract class MonsterState : MonoBehaviour
{
    protected MonsterFSMController controller;

    protected Animator animator;

    protected NavMeshAgent navMeshAgent;

    protected MonsterFSMInfo fsmInfo;

    [Range(1f, 2f)]
    [SerializeField] protected float animSpeed;

    protected virtual void Awake()
    {
        controller = GetComponent<MonsterFSMController>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        fsmInfo = GetComponent<MonsterFSMInfo>();
    }

    public virtual void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        animator.speed = animSpeed;
    }

    public abstract void UpdateState();

    public abstract void ExitState();

    protected virtual void NavigationStop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0f;
    }
}
