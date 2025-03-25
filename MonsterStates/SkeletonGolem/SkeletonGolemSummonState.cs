using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGolemSummonState : SkeletonGolemState
{
    [SerializeField] GameObject[] summonPrefabs;
    [SerializeField] GameObject summonEffectPrefab;

    public override void EnterState(SkeletonGolemFSMController.STATE state, object data = null)
    {
        animator.SetInteger("State", (int)state);
        fsmInfo.CanSummon = false;
        SoundManager.Instance.PlayBossSound(SoundManager.BOSSSOUND.SUMMONSTART);

        time = 0f;
    }

    public override void ExitState()
    {
        time = 0f;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        if (time > 5f)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.IDLE);
            return;
        }
    }

    private void SummonSkeleton()
    {
        int summonCount = Random.Range((int)fsmInfo.SummonMonsterRange.x, (int)fsmInfo.SummonMonsterRange.y);
        SoundManager.Instance.PlayBossSound(SoundManager.BOSSSOUND.SUMMONEND);

        for (int i = 0; i < summonCount; i++)
        {
            int rand = Random.Range(0, 10);
            Vector3 randomPosition = Random.insideUnitSphere * fsmInfo.SummonRadius;
            Vector3 summonPosition = transform.position + new Vector3(randomPosition.x, 0f, randomPosition.z);

            if (rand < 5)
            {
                Instantiate(summonEffectPrefab, summonPosition, Quaternion.Euler(-90f, 0f, 0f));
                Instantiate(summonPrefabs[0], summonPosition, Quaternion.identity);
            }
            else if (rand < 7)
            {
                Instantiate(summonEffectPrefab, summonPosition, Quaternion.Euler(-90f, 0f, 0f));
                Instantiate(summonPrefabs[1], summonPosition, Quaternion.identity);
            }
            else if (rand < 9)
            {
                Instantiate(summonEffectPrefab, summonPosition, Quaternion.Euler(-90f, 0f, 0f));
                Instantiate(summonPrefabs[2], summonPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(summonEffectPrefab, summonPosition, Quaternion.Euler(-90f, 0f, 0f));
                Instantiate(summonPrefabs[3], summonPosition, Quaternion.identity);
            }
        }
    }
}
