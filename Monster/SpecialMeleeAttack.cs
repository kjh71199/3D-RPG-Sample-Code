using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMeleeAttack : MeleeAttack
{
    public void SpecialMeleeAttackHitAnimationEvent()
    {
        SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.ATTACK3);
        RangeAngleTargetsAttack();
    }
}
