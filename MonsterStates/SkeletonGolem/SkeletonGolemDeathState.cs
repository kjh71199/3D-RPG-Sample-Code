using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonGolemDeathState : SkeletonGolemState
{
    [SerializeField] protected float deathDelayTime;
    [SerializeField] protected GameObject destroyParticlePrefab;
    [SerializeField] protected Transform destroyParticleTr;

    [SerializeField] protected float knockbackTime;
    [SerializeField] protected float knockbackForce;

    public override void EnterState(SkeletonGolemFSMController.STATE state, object data = null)
    {
        time = 0f;
        animator.SetInteger("State", 0);
        animator.SetBool("Dead", true);
        navMeshAgent.isStopped = true;
        SoundManager.Instance.PlayBossSound(SoundManager.BOSSSOUND.DEATH);

        Stats playerStat = GameObject.FindWithTag("Player").GetComponent<Stats>();
        playerStat.exp += fsmInfo.Stats.exp;

        float force = knockbackForce;
        if (data != null)
        {
            force = (float)data;
        }
    }

    public override void ExitState()
    {
        time = 0f;
        DungeonEventManager.bossRoomClear();

        SoundManager.Instance.PlayBossSound(SoundManager.BOSSSOUND.DESTROY);
        Instantiate(destroyParticlePrefab, destroyParticleTr.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time > deathDelayTime)
        {
            ExitState();
        }
    }
}
