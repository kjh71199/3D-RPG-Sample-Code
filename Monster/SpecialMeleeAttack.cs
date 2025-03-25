using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 특수 근접 공격 애니메이션 이벤트 컴포넌트
public class SpecialMeleeAttack : MeleeAttack
{
    public void SpecialMeleeAttackHitAnimationEvent()
    {
        SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.ATTACK3);
        RangeAngleTargetsAttack();
    }
}
