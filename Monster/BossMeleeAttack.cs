using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스 근접 공격 애니메이션 이벤트 컴포넌트
public class BossMeleeAttack : MeleeAttack
{
    public void BossMeleeAttackHitAnimationEvent()
    {
        SoundManager.Instance.PlayBossSound(SoundManager.BOSSSOUND.ATTACK2);
        RangeAngleTargetsAttack();
    }
}
