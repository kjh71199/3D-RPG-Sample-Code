using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : Health
{
    [SerializeField] protected CharacterStatSO stats;

    private MonsterFSMController controller;

    protected void Awake()
    {
        controller = GetComponent<MonsterFSMController>();
    }

    private void Start()
    {
        MaxHp = stats.maxHp;
        CurrentHp = MaxHp;
    }

    public override void Hit(float damage, float knockbackForce, Vector3 direction, Enums.CROWDCONTROL control)
    {
        CurrentHp -= (int)damage;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);

        if (CurrentHp <= 0)
        {
            controller.TransactionToState(MonsterFSMController.STATE.DEATH, knockbackForce);
        }
        else if (control == Enums.CROWDCONTROL.NONE)
        {
            controller.TransactionToState(MonsterFSMController.STATE.HIT, knockbackForce);
        }
        else if (control == Enums.CROWDCONTROL.STUN)
        {
            float stunTime = knockbackForce;
            controller.TransactionToState(MonsterFSMController.STATE.STUN, stunTime);
        }
    }
}
