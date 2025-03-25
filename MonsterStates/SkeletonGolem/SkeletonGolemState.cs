using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 보스 FSM 상태 추상 클래스
public abstract class SkeletonGolemState : MonoBehaviour
{
    protected SkeletonGolemFSMController controller;

    protected Animator animator;

    protected NavMeshAgent navMeshAgent;

    protected SkeletonGolemFSMInfo fsmInfo;

    protected float time;

    protected virtual void Awake()
    {
        controller = GetComponent<SkeletonGolemFSMController>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        fsmInfo = GetComponent<SkeletonGolemFSMInfo>();
    }

    public abstract void EnterState(SkeletonGolemFSMController.STATE state, object data = null);

    public abstract void UpdateState();

    public abstract void ExitState();

    protected virtual void NavigationStop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0f;
    }
}
