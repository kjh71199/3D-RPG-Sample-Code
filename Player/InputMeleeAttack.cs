using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMeleeAttack : MonoBehaviour
{
    private MeleeAttack meleeAttack;

    public bool isAttackInput;

    private Animator animator;
    private InputNavmeshMovement movement;

    private Collider targetCollider;
    private Vector3 attackPosition;

    private int hashStateTime = Animator.StringToHash("StateTime");

    private int hashCombo1 = Animator.StringToHash("Combo1");
    private int hashCombo2 = Animator.StringToHash("Combo2");
    private int hashCombo3 = Animator.StringToHash("Combo3");

    private AnimatorStateInfo currentStateInfo;
    public float animationNormalTime;

    public Collider TargetCollider { get => targetCollider; set => targetCollider = value; }
    public Animator Animator { get => animator; set => animator = value; }

    private void Awake()
    {
        meleeAttack = GetComponent<MeleeAttack>();
        Animator = GetComponent<Animator>();
        movement = GetComponent<InputNavmeshMovement>();
    }

    void Update()
    {
        if (targetCollider != null)
        {
            movement.SetAgentDestination(targetCollider.transform.position - transform.forward);
            animator.SetBool("Attack", true);

            if (Vector3.Distance(transform.position, targetCollider.transform.position) <= meleeAttack.AttackRadius * 2)
            {
                movement.MovePosition = transform.position;

                Animator.SetTrigger(StringToHash.MeleeAttack);
                attackPosition = targetCollider.transform.position;
                movement.MovePosition = transform.position;
                transform.rotation = Quaternion.LookRotation((attackPosition - transform.position).normalized, Vector3.up);
            }
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        animationNormalTime = Mathf.Repeat(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);
        Animator.SetFloat(hashStateTime, animationNormalTime);
    }


    public void TouchInputAttack(Collider collider)
    {
        targetCollider = collider;
    }

    public bool IsPlayAttackAnimation()
    {
        currentStateInfo = Animator.GetCurrentAnimatorStateInfo(0);

        if (currentStateInfo.shortNameHash == hashCombo1 ||
            currentStateInfo.shortNameHash == hashCombo2 ||
            currentStateInfo.shortNameHash == hashCombo3)
            return true;

        return false;
    }
}
