using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLeap : Skill
{
    private DamageSkillSO damageData;
    private Vector3 targetPosition;

    [SerializeField] private LayerMask groundLayerMask;

    private float peakHeight = 10f;

    private bool isHit;

    protected override void Awake()
    {
        base.Awake();
        damageData = (DamageSkillSO)Data;
    }

    public override void PerformSkill()
    {
        ActivateLeaf();
    }

    private IEnumerator LeafCoroutine(Vector3 start, Vector3 target)
    {
        float time = 0f;
        Vector3 midpoint = new Vector3((start.x + target.x) * 0.5f, Mathf.Max(start.y, target.y) + peakHeight, (start.z + target.z) *0.5f);
        movement.SetAgentDestination(target);
        animator.SetBool(damageData.skillName, true);

        // ������ ����Ǵ� ���� �̵�
        while (time < damageData.skillPerformTime)
        {
            time += Time.deltaTime;
            float t = time / damageData.skillPerformTime;

            // ������ ���: �� ���� �� (start, midpoint, target)�� �̿��� Bezier � ���
            Vector3 position = BezierCurve(start, midpoint, target, t);
            transform.position = position;
            yield return null;
        }

        // ������ ������ �� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
        transform.position = target;
        movement.MovePosition = transform.position;
        animator.SetBool(damageData.skillName, false);
        soundFx.PlaySound(damageData.skillSoundFx);
        LeafPerformDamage();
    }

    // 3���� ���� �̿��� Bezier � ���
    private Vector3 BezierCurve(Vector3 start, Vector3 midpoint, Vector3 end, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * start; // (1-t)^2 * start
        p += 2 * u * t * midpoint; // 2(1-t)t * midpoint
        p += tt * end; // t^2 * end
        return p;
    }

    private void ActivateLeaf()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
            targetPosition = hit.point;  // 3D ���������� ���콺 ������ ��ġ;
        else 
            targetPosition = transform.position;

        Vector3 startPosition = transform.position;
        StartCoroutine(LeafCoroutine(startPosition, targetPosition));
    }

    private void LeafPerformDamage()
    {
        effectParticle.Play();
        isHit = false;

        Collider[] hits = Physics.OverlapSphere(transform.position, damageData.skillRange, damageData.targetLayer);

        foreach (Collider hit in hits)
        {
            isHit = true;
            Vector3 directionToTarget = hit.transform.position - transform.position;
            directionToTarget = new Vector3(directionToTarget.x, transform.position.y, directionToTarget.z);

            hit.GetComponent<Health>().Hit((int)(stats.attack * damageData.skillDamageMultiplier), damageData.skillKnockbackForce, directionToTarget, Enums.CROWDCONTROL.NONE);
        }

        if (isHit)
        {
            SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.HIT);
            isHit = false;
        }
    }
}
