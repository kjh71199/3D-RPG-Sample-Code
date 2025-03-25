using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� FSM ���� ��ġ ���� ������Ʈ
public class MonsterFSMInfo : MonoBehaviour
{
    [SerializeField] private CharacterStatSO stats;

    [Header("����")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackDelayTime;

    [Header("����")]
    [SerializeField] private float detectDistance;
    [SerializeField] private float detectMoveSpeedModifier;

    [Header("�ι�")]
    [SerializeField] private float nextPointSelectDistance;
    [SerializeField] private Transform[] wanderPoints;

    [Header("��ȸ")]
    [SerializeField] private float wanderMoveSpeedModifier;
    [SerializeField] private float wanderNavCheckRadius;

    [Header("��")]
    [SerializeField] private float giveUpMoveSpeedModifier;

    public CharacterStatSO Stats { get => stats; set => stats = value; }
    public float AttackDistance { get => attackDistance; set => attackDistance = value; }
    public float AttackDelayTime { get => attackDelayTime; set => attackDelayTime = value; }
    public float DetectDistance { get => detectDistance; set => detectDistance = value; }
    public float DetectMoveSpeedModifier { get => detectMoveSpeedModifier; set => detectMoveSpeedModifier = value; }
    public float NextPointSelectDistance { get => nextPointSelectDistance; set => nextPointSelectDistance = value; }
    public Transform[] WanderPoints { get => wanderPoints; set => wanderPoints = value; }
    public float WanderMoveSpeedModifier { get => wanderMoveSpeedModifier; set => wanderMoveSpeedModifier = value; }
    public float WanderNavCheckRadius { get => wanderNavCheckRadius; set => wanderNavCheckRadius = value; }
    public float GiveUpMoveSpeedModifier { get => giveUpMoveSpeedModifier; set => giveUpMoveSpeedModifier = value; }
}
