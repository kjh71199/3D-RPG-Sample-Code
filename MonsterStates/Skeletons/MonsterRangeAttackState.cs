using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangeAttackState : MonsterState
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform shootTransform;
    [SerializeField] protected int maxProjectileCount;

    protected GameObject[] projectiles;
    protected SkeletonProjectile[] projectileSet;

    public GameObject[] Projectiles { get => projectiles; set => projectiles = value; }
    public int MaxProjectileCount { get => maxProjectileCount; set => maxProjectileCount = value; }

    protected override void Awake()
    {
        base.Awake();

        Projectiles = new GameObject[maxProjectileCount];
        projectileSet = new SkeletonProjectile[maxProjectileCount];

        for (int i = 0; i < MaxProjectileCount; i++)
        {
            Projectiles[i] = Instantiate(projectilePrefab);
            projectileSet[i] = Projectiles[i].GetComponent<SkeletonProjectile>();
            Projectiles[i].SetActive(false);
        }
    }

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        NavigationStop();

        animator.SetInteger("State", (int)state);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (controller.GetPlayerDistance() > fsmInfo.AttackDistance)
        {
            controller.TransactionToState(MonsterFSMController.STATE.GIVEUP);
            return;
        }

        LookAtTarget();
    }

    protected void LookAtTarget()
    {
        Vector3 direction = (controller.Player.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * fsmInfo.Stats.rotateSpeed);
    }

    protected virtual void ShootProjectile()
    {
        SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.ATTACK1);
        for (int i = 0; i < MaxProjectileCount; i++)
        {
            if (Projectiles[i].activeSelf == false)
            {
                Projectiles[i].transform.position = shootTransform.position;
                projectileSet[i].Direction = transform.forward;
                projectileSet[i].Damage = (int)fsmInfo.Stats.attack;
                Projectiles[i].SetActive(true);
                return;
            }
        }
    }
}
