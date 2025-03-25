using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathBombState : MonsterDeathState
{
    [SerializeField] protected float bombDelayTime;
    [SerializeField] protected ParticleSystem bombDelayParticle;
    [SerializeField] protected ParticleSystem bombParticle;
    [SerializeField] protected float bombRadius;
    [SerializeField] protected SpriteRenderer bombCircleRenderer;
    [SerializeField] protected LayerMask targetLayer;

    Stats stats;
    protected bool isBomb = false;
    protected object data;
    protected Vector3 bombParticlePosition;

    protected override void Awake()
    {
        base.Awake();
        stats = GetComponent<Stats>();
    }

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        hitParticle.Play();
        animator.SetBool("Dead", true);
        StartCoroutine(BombCircleCoroutine());
        NavigationStop();
        bombDelayParticle.Play();
        myCollider.enabled = false;
    }

    public override void ExitState()
    {
        if (IsSpawned)
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

        if (time > bombDelayTime && !isBomb)
        {
            isBomb = true;
            time = 0;
            animator.SetBool("Bomb", true);
            bombCircleRenderer.enabled = false;

            float force = knockbackForce;
            if (data != null)
            {
                force = (float)data;
            }

            bombParticle.Play();
            bombDelayParticle.Clear();
            BombDamage();
            SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.DEATHBOMB);
            StartCoroutine(ApplyDeathKnockback(-transform.forward, force));
        }

        if (time > deathDelayTime && isBomb)
        {
            ExitState();
        }
    }

    protected void BombDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, bombRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = hit.transform.position - transform.position;
            directionToTarget = new Vector3(directionToTarget.x, transform.position.y, directionToTarget.z);

            Debug.Log($"{hit.name}를 타격함");

            hit.GetComponent<Health>().Hit(stats.attack * 2f, knockbackForce, directionToTarget, Enums.CROWDCONTROL.NONE);
        }
    }

    protected IEnumerator BombCircleCoroutine()
    {
        float time = 0;
        bombCircleRenderer.enabled = true;
        bombCircleRenderer.color = new Color(1f, 1f, 1f, 0f);

        while (time <= 0.5f)
        {
            time += Time.deltaTime;

            bombCircleRenderer.color = new Color(1f, 1f, 1f, time * 2f);

            yield return null;
        }
    }

    protected override IEnumerator ApplyDeathKnockback(Vector3 hitDirection, float force)
    {
        navMeshAgent.isStopped = true;

        float timer = 0f;
        while (timer < knockbackTime)
        {
            navMeshAgent.Move(hitDirection * force * Time.deltaTime);
            bombParticle.transform.Translate(-hitDirection * force * Time.deltaTime, Space.World);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
