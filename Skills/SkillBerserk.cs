using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBerserk : Skill
{
    BuffSkillSO buffData;
    private Coroutine buffCoroutine;

    protected override void Awake()
    {
        base.Awake();
        buffData = (BuffSkillSO)Data;
    }

    public override void PerformSkill()
    {
        buffCoroutine = StartCoroutine(BerserkCoroutine());
    }

    public override void DeActivateSkill()
    {
        StopCoroutine(buffCoroutine);

        foreach (var pair in buffData.resetStats)
        {
            Stats.STATS stat = pair.Key;
            float value = pair.Value;
            stats.ResetStat(stat, value);
        }

        effectParticle.Stop();
        effectParticle.Clear();
        buffData.resetStats.Clear();
    }

    private IEnumerator BerserkCoroutine()
    {
        effectParticle.Play();
        soundFx.PlaySound(buffData.skillSoundFx);

        foreach (var pair in buffData.skillUpValues)
        {
            Stats.STATS stat = pair.Key;
            float value = pair.Value;
            
            buffData.resetStats.Add(stat, stats.GetStat(stat));
            stats.StatBuff(stat, value);
        }
        
        yield return new WaitForSeconds(buffData.skillDurationTime);

        foreach (var pair in buffData.resetStats)
        {
            Stats.STATS stat = pair.Key;
            float value = pair.Value;
            stats.ResetStat(stat, value);
        }

        effectParticle.Stop();
        effectParticle.Clear();
        buffData.resetStats.Clear();
    }
}
