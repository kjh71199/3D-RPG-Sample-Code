using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 FSM 관련 수치 관리 컴포넌트
public class MonsterFSMInfo : MonoBehaviour
{
    [SerializeField] private CharacterStatSO stats;

    [Header("공격")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackDelayTime;

    [Header("추적")]
    [SerializeField] private float detectDistance;
    [SerializeField] private float detectMoveSpeedModifier;

    [Header("로밍")]
    [SerializeField] private float nextPointSelectDistance;
    [SerializeField] private Transform[] wanderPoints;

    [Header("배회")]
    [SerializeField] private float wanderMoveSpeedModifier;
    [SerializeField] private float wanderNavCheckRadius;

    [Header("퇴각")]
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
