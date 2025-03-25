using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 휴식 상태 컴포넌트
public class MonsterRestState : MonsterState
{
    protected Health health;
    private IEnumerator hpRegenCoroutine;

    [SerializeField] private GameObject bonfire;
    [SerializeField] private float restDistance;

    public GameObject Bonfire { get => bonfire; set => bonfire = value; }
    public float RestDistance { get => restDistance; set => restDistance = value; }

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Health>();
    }

    public override void EnterState(MonsterFSMController.STATE state, object data = null)
    {
        base.EnterState(state, data);

        Vector3 direction = (Bonfire.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        animator.SetInteger("State", (int)state);

        hpRegenCoroutine = HpRegenCoroutine();
        StartCoroutine(hpRegenCoroutine);
    }

    public override void ExitState()
    {
        StopCoroutine(hpRegenCoroutine);
    }

    public override void UpdateState()
    {
        if (controller.GetPlayerDistance() <= fsmInfo.AttackDistance)
        {
            controller.TransactionToState(MonsterFSMController.STATE.ATTACK);
            return;
        }

        if (controller.GetPlayerDistance() <= fsmInfo.DetectDistance)
        {
            controller.TransactionToState(MonsterFSMController.STATE.DETECT);
            return;
        }

        if (Vector3.Distance(Bonfire.transform.position, transform.position) < 1.1f)
        {
            navMeshAgent.Move(-transform.forward * Time.deltaTime);
        }
    }

    private IEnumerator HpRegenCoroutine()
    {
        while (health.CurrentHp < health.MaxHp)
        {
            yield return new WaitForSeconds(5f);
            health.CurrentHp++;
        }
    }
}
