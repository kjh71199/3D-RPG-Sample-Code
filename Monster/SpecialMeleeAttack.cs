using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� Ư�� ���� ���� �ִϸ��̼� �̺�Ʈ ������Ʈ
public class SpecialMeleeAttack : MeleeAttack
{
    public void SpecialMeleeAttackHitAnimationEvent()
    {
        SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.ATTACK3);
        RangeAngleTargetsAttack();
    }
}
