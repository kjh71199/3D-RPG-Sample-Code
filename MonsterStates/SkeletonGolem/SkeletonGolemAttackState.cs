using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스 공격 상태 컴포넌트
public class SkeletonGolemAttackState : SkeletonGolemState
{
    private float animationTime;
    [SerializeField] private TrailRenderer trailRenderer;

    private bool isAttack;

    public override void EnterState(SkeletonGolemFSMController.STATE state, object data = null)
    {
        animator.SetInteger("State", (int)state);
        
        NavigationStop();
        time = 0f;
        isAttack = true;

        animator.speed = 1f;
    }

    public override void ExitState()
    {
        time = 0f;
        animator.speed = 1f;
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;
        animationTime = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);

        if (animationTime > 0.4f)
        {
            animator.speed = 1.5f;
        }

        if (animationTime > 0.9f)
        {
            isAttack = false;
            //controller.TransactionToState(SkeletonGolemFSMController.STATE.IDLE);
            //return;
        }

        if (!isAttack)
        {
            TrailOff();
            animator.speed = 1f;
            controller.TransactionToState(SkeletonGolemFSMController.STATE.IDLE);
            return;
        }

        LookAtTarget();
    }

    protected void LookAtTarget()
    {
        Vector3 direction = (controller.Player.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * fsmInfo.Stats.rotateSpeed);
    }

    protected void TrailOn()
    {
        trailRenderer.enabled = true;
    }

    protected void TrailOff()
    {
        trailRenderer.enabled = false;
    }

    public void PlayAttackSoundFx()
    {
        SoundManager.Instance.PlayBossSound(SoundManager.BOSSSOUND.ATTACK1);
    }
}
