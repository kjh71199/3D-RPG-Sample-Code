using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGolemStunState : SkeletonGolemState
{
    [SerializeField] private ParticleSystem stunParticle;

    private float stunTime;

    public override void EnterState(SkeletonGolemFSMController.STATE state, object data = null)
    {
        time = 0f;

        animator.SetBool("Stun", true);
        animator.SetInteger("State", (int)state);

        if (data != null)
        {
            stunTime = (float)data * 0.5f;
        }

        time = 0f;
        navMeshAgent.isStopped = true;
        stunParticle.Play();
    }

    public override void ExitState()
    {
        animator.SetBool("Stun", false);
        navMeshAgent.isStopped = false;
        stunParticle.Stop();
        stunParticle.Clear();
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time >= stunTime)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.DETECT);
            return;
        }
    }
}
