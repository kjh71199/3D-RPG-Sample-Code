using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���� �ִϸ��̼� �̺�Ʈ ������Ʈ
public class BossMeleeAttack : MeleeAttack
{
    public void BossMeleeAttackHitAnimationEvent()
    {
        SoundManager.Instance.PlayBossSound(SoundManager.BOSSSOUND.ATTACK2);
        RangeAngleTargetsAttack();
    }
}
