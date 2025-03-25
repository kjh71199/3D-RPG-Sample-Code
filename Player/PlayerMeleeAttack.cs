using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : NormalMeleeAttack
{
    private bool isHit;

    public override void RangeAngleTargetsAttack()
    {
        Collider[] hits = Physics.OverlapSphere(attackTransform.position, AttackRadius, targetLayer);
        isHit = false;

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;

            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < hitAngle)
            {
                hit.GetComponent<Health>().Hit(damage, knockbackForce, directionToTarget, Enums.CROWDCONTROL.NONE);
                isHit = true;
            }
        }

        if (attackParticle != null)
            attackParticle.Play();

        if (isHit)
        {
            SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.HIT);
            isHit = false;
        }
    }
    public void PlayerMeleeAttackHitAnimationEvent()
    {
        RangeAngleTargetsAttack();
    }
}
