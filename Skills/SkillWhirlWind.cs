using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWhirlWind : Skill
{
    private WaitForSeconds startDelay;
    private DamageSkillSO damageData;

    private bool isHit;

    protected override void Awake()
    {
        base.Awake();
        startDelay = new WaitForSeconds(0.4f);
        damageData = (DamageSkillSO)Data;
    }

    public override void PerformSkill()
    {
        StartCoroutine(WhirlWindCoroutine());
    }

    private IEnumerator WhirlWindCoroutine()
    {
        float time = 0f;
        float attackTick = 0f;

        controller.IsRotationLocked = true;
        animator.SetBool(data.skillName, true);

        yield return startDelay;
        effectParticle.Play();
        soundFx.PlaySound(damageData.skillSoundFx);

        while (time < damageData.skillPerformTime)
        {
            time += Time.deltaTime;
            attackTick += Time.deltaTime;

            if (attackTick >= damageData.skillDamageTickTime)
            {
                attackTick = 0f;
                WhirlWindPerformDamage();
            }

            yield return null;
        }

        controller.IsRotationLocked = false;
        animator.SetBool(data.skillName, false);
        effectParticle.Stop();
    }

    private void WhirlWindPerformDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, damageData.skillRange, damageData.targetLayer);
        soundFx.PlaySound(damageData.skillSoundFx);
        isHit = false;

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = hit.transform.position - transform.position;
            directionToTarget = new Vector3(directionToTarget.x, transform.position.y, directionToTarget.z);
            isHit = true;

            hit.GetComponent<Health>().Hit((int)(stats.attack * damageData.skillDamageMultiplier), damageData.skillKnockbackForce, directionToTarget, Enums.CROWDCONTROL.NONE);
        }

        if (isHit)
        {
            SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.HIT);
            isHit = false;
        }
    }

}
