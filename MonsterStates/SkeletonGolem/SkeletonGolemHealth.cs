using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGolemHealth : Health
{
    private SkeletonGolemFSMController controller;
    private WaitForSeconds hitWait;

    [SerializeField] protected CharacterStatSO stats;

    [SerializeField] private Renderer[] meshRenderers;

    private Material originalMaterial;
    [SerializeField] private Material hitMaterial;

    protected void Awake()
    {
        controller = GetComponent<SkeletonGolemFSMController>();
        hitWait = new WaitForSeconds(0.1f);

        originalMaterial = meshRenderers[0].material;
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
        BossUIManager.bossHpUpdate();
        StartCoroutine(HitCoroutine());

        if (CurrentHp <= 0)
        {
            controller.TransactionToState(SkeletonGolemFSMController.STATE.DEATH, knockbackForce);
        }
        else if (control == Enums.CROWDCONTROL.STUN)
        {
            float stunTime = knockbackForce;
            controller.TransactionToState(SkeletonGolemFSMController.STATE.STUN, stunTime);
        }
    }

    private IEnumerator HitCoroutine()
    {
        foreach (Renderer renderer in meshRenderers)
        {
            renderer.material = hitMaterial;
        }
        yield return hitWait;
        foreach (Renderer renderer in meshRenderers)
        {
            renderer.material = originalMaterial;
        }
    }
}
