using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� FSM ���� ��ġ ���� ������Ʈ
public class SkeletonGolemFSMInfo : MonoBehaviour
{
    [SerializeField] private CharacterStatSO stats;

    [Header("����")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackDelayTime;

    [Header("Ư������")]
    [SerializeField] private float specialAttackDistance;
    [SerializeField] private float specialAttackCooldownTime;
    [SerializeField] private float attackMoveSpeedModifier;
    private bool canSpecialAttack = false;
    private WaitForSeconds specialAttackWait;

    [Header("����")]
    [SerializeField] private float detectMoveSpeedModifier;

    [Header("��ȯ")]
    [SerializeField] private float summonCooldownTime;
    [SerializeField] private float summonRadius;
    [SerializeField] private Vector2 summonMonsterRange;
    private bool canSummon = true;
    private WaitForSeconds summonWait;


    public CharacterStatSO Stats { get => stats; set => stats = value; }
    public float AttackDistance { get => attackDistance; set => attackDistance = value; }
    public float AttackDelayTime { get => attackDelayTime; set => attackDelayTime = value; }
    public float SpecialAttackDistance { get => specialAttackDistance; set => specialAttackDistance = value; }
    public float SpecialAttackCooldownTime { get => specialAttackCooldownTime; set => specialAttackCooldownTime = value; }
    public float AttackMoveSpeedModifier { get => attackMoveSpeedModifier; set => attackMoveSpeedModifier = value; }
    public bool CanSpecialAttack { get => canSpecialAttack; set => canSpecialAttack = value; }
    public float DetectMoveSpeedModifier { get => detectMoveSpeedModifier; set => detectMoveSpeedModifier = value; }
    public float SummonCooldownTime { get => summonCooldownTime; set => summonCooldownTime = value; }
    public float SummonRadius { get => summonRadius; set => summonRadius = value; }
    public Vector2 SummonMonsterRange { get => summonMonsterRange; set => summonMonsterRange = value; }
    public bool CanSummon { get => canSummon; set => canSummon = value; }

    private void Awake()
    {
        specialAttackWait = new WaitForSeconds(specialAttackCooldownTime);
        summonWait = new WaitForSeconds(summonCooldownTime);

        StartCoroutine(SpecialAttackCooldown());
        StartCoroutine(SummonCooldown());
    }

    private IEnumerator SpecialAttackCooldown()
    {
        while (true)
        {
            if (!CanSpecialAttack)
            {
                yield return specialAttackWait;
                CanSpecialAttack = true;
            }
            yield return null;
        }
    }

    private IEnumerator SummonCooldown()
    {
        while (true)
        {
            if (!CanSummon)
            {
                yield return summonWait;
                CanSummon = true;
            }
            yield return null;
        }
    }
}
