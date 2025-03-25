using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStunState : MonsterState
{
    [SerializeField] private float stunTime;
    [SerializeField] private ParticleSystem stunParticle;

    float time;

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        animator.SetBool("Stun", true);
        animator.SetInteger("State", (int)state);

        if (data != null)
        {
            stunTime = (float)data;
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
            controller.TransactionToState(MonsterFSMController.STATE.DETECT);
            return;
        }
    }
}
