using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 마법 공격 상태 컴포넌트
public class MonsterMagicAttackState : MonsterRangeAttackState
{
    [SerializeField] protected float castTime;
    protected float time;

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        NavigationStop();

        animator.SetInteger("State", (int)state);

        time = 0f;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time >= castTime)
        {
            animator.SetTrigger("CastEnd");
            time = 0f;
        }

        base.UpdateState();
    }

    public override void ExitState()
    {
        time = 0f;
        animator.ResetTrigger("CastEnd");
    }

    protected override void ShootProjectile()
    {
        SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.ATTACK2);
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
