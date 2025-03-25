using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 발구르기 스킬 컴포넌트
public class SkillGroundStomp : Skill
{
    private DamageSkillSO damageData;

    private float hopHeight = 1f;

    private bool isHit;

    protected override void Awake()
    {
        base.Awake();
        damageData = (DamageSkillSO)Data;
    }

    public override void PerformSkill()
    {
        StartCoroutine(StompCoroutine());
    }

    private IEnumerator StompCoroutine()
    {
        float time = 0f;
        bool damage = false;
        Vector3 startPosition = transform.position;
        movement.MovePosition = startPosition;

        transform.position += new Vector3(0f, hopHeight, 0f);
        animator.SetTrigger(data.skillName);

        float heightDelta = hopHeight / damageData.skillPerformTime;

        while (time <= damageData.skillPerformTime)
        {
            time += Time.deltaTime;
            transform.position -= new Vector3(0f, heightDelta * Time.deltaTime, 0f);

            if (!damage && transform.position.y - startPosition.y <= 0.01)
            {
                StompPerformDamage();
                soundFx.PlaySound(damageData.skillSoundFx);
                damage = true;
            }

            yield return null;
        }
    }

    private void StompPerformDamage()
    {
        effectParticle.Play();
        Collider[] hits = Physics.OverlapSphere(transform.position, damageData.skillRange, damageData.targetLayer);
        isHit = false;

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = hit.transform.position - transform.position;
            directionToTarget = new Vector3(directionToTarget.x, transform.position.y, directionToTarget.z);
            isHit = true;

            hit.GetComponent<Health>().Hit((int)(stats.attack * damageData.skillDamageMultiplier), damageData.skillKnockbackForce, directionToTarget, Enums.CROWDCONTROL.STUN);
        }

        if (isHit)
        {
            SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.HIT);
            isHit = false;
        }
    }
}
