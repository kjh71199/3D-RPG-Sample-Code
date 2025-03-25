using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 사망 상태 컴포넌트
public class MonsterDeathState : MonsterState
{
    protected float time;
    protected Collider myCollider;

    [SerializeField] protected float deathDelayTime;
    [SerializeField] protected ParticleSystem hitParticle;
    [SerializeField] protected GameObject destroyParticlePrefab;
    [SerializeField] protected Transform destroyParticleTr;

    [SerializeField] protected float knockbackTime;
    [SerializeField] protected float knockbackForce;

    private bool isSpawned = false;

    public bool IsSpawned { get => isSpawned; set => isSpawned = value; }

    protected override void Awake()
    {
        base.Awake();
        myCollider = GetComponent<Collider>();
    }

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        hitParticle.Play();
        animator.SetBool("Dead", true);
        myCollider.enabled = false;

        Stats playerStat = GameObject.FindWithTag("Player").GetComponent<Stats>();
        playerStat.exp += fsmInfo.Stats.exp;

        float force = knockbackForce;
        if (data != null)
        {
            force = (float)data;
        }

        StartCoroutine(ApplyDeathKnockback(-transform.forward, force));
        SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.DEATH);
    }

    public override void ExitState()
    {
        MonsterRangeAttackState range = GetComponent<MonsterRangeAttackState>();

        if (range != null)
        {
            for (int i = 0; i < range.MaxProjectileCount; i++)
            {
                Destroy(range.Projectiles[i]);
            }
        }

        if (isSpawned)
        {
            DungeonSpawner spawner = GameObject.FindWithTag("Spawner").GetComponent<DungeonSpawner>();
            spawner.MonsterDeathCount(gameObject);
        }

        SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.DESTROY);
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

    protected virtual IEnumerator ApplyDeathKnockback(Vector3 hitDirection, float force)
    {
        navMeshAgent.isStopped = true;

        float timer = 0f;
        while (timer < knockbackTime)
        {
            navMeshAgent.Move(hitDirection * force * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}

