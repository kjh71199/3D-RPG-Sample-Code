using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitState : MonsterState
{
    [SerializeField] private bool isHit;
    public bool IsHit { get => isHit; set => isHit = value; }

    [SerializeField] protected ParticleSystem hitParticle;

    private MonsterHealth health;

    [SerializeField] protected float knockbackTime;
    [SerializeField] protected float knockbackForce;

    protected override void Awake()
    {
        base.Awake();

        health = GetComponent<MonsterHealth>();
    }

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        float force = knockbackForce;
        if (data != null)
        {
            force = (float)data;
        }

        navMeshAgent.isStopped = true;

        hitParticle.Play();

        animator.SetInteger("State", (int)state);

        StartCoroutine(ApplyHitKnockback(-transform.forward, force));
    }

    public override void ExitState()
    {
        health.IsHit = false;
    }

    public override void UpdateState()
    {
        if (health.IsHit) return;

        if (controller.GetPlayerDistance() <= fsmInfo.AttackDistance)
        {
            controller.TransactionToState(MonsterFSMController.STATE.ATTACK);
            return;
        }
        else
        {
            controller.TransactionToState(MonsterFSMController.STATE.DETECT);
            return;
        }
    }

    private IEnumerator ApplyHitKnockback(Vector3 hitDirection, float force)
    {
        health.IsHit = true;
        navMeshAgent.isStopped = true;

        float timer = 0f;
        while (timer < knockbackTime)
        {
            navMeshAgent.Move(hitDirection * force * Time.fixedDeltaTime);
            timer += Time.fixedDeltaTime;
            yield return null;
        }

        navMeshAgent.isStopped = false;
        health.IsHit = false;
    }
}
