using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSteelGrasp : Skill
{
    private DamageSkillSO damageData;
    private Vector3 targetPosition;

    [SerializeField] private float hitAngle;
    [SerializeField] private Transform pullTransform;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private GameObject sicklePrefab;

    private GameObject[] sicklePrefabs = new GameObject[4];
    private float[] angles = new float[] { -30f, -10f, 10f, 30f };
    private WaitForSeconds waitForSeconds;

    private bool isHit;

    protected override void Awake()
    {
        base.Awake();
        damageData = (DamageSkillSO)Data;
        waitForSeconds = new WaitForSeconds(damageData.skillPerformTime);

        for (int i = 0; i < 4; i++)
        {
            sicklePrefabs[i] = Instantiate(sicklePrefab, transform.position, Quaternion.identity);
            sicklePrefabs[i].transform.SetParent(skillEffectTransform);
            SickleMovement move = sicklePrefabs[i].GetComponent<SickleMovement>();
            move.Range = damageData.skillRange;
            move.Player = gameObject;
            move.Direction = Vector3.zero;
            sicklePrefabs[i].SetActive(false);
        }
    }

    public override void PerformSkill()
    {
        ActivateSteelGrasp();
    }

    private void ActivateSteelGrasp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            targetPosition = hit.point;  // 3D 공간에서의 마우스 포인터 위치;
        }

        StartCoroutine(MovementLockCoroutine());
        movement.Agent.ResetPath();

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;
        Vector3.Normalize(direction);
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;

        for (int i = 0; i < sicklePrefabs.Length; i++)
        {
            SickleMovement move = sicklePrefabs[i].GetComponent<SickleMovement>();
            move.Direction = (Quaternion.Euler(0f, angles[i], 0f) * direction).normalized;
            sicklePrefabs[i].SetActive(true);
        }

        animator.SetTrigger(damageData.skillName);
    }

    private void PerformSteelGraspDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, damageData.skillRange, damageData.targetLayer);
        soundFx.PlaySound(damageData.skillSoundFx);
        isHit = false;

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
            
            if (angleToTarget < hitAngle)
            {
                Health health = hit.GetComponent<Health>();
                if (health != null)
                {
                    isHit = true;
                    health.Hit((int)(stats.attack * damageData.skillDamageMultiplier), damageData.skillKnockbackForce, directionToTarget, Enums.CROWDCONTROL.STUN);

                    if (!health.IsGiant)
                        StartCoroutine(PullEnemyCoroutine(hit.gameObject));
                }
            }
        }

        if (isHit)
        {
            SoundManager.Instance.PlayMonsterSound(SoundManager.MONSTERSOUND.HIT);
            isHit = false;
        }
    }

    private IEnumerator MovementLockCoroutine()
    {
        controller.IsRotationLocked = true;
        controller.IsMovementLocked = true;
        yield return waitForSeconds;
        controller.IsRotationLocked = false;
        controller.IsMovementLocked = false;
    }

    private IEnumerator PullEnemyCoroutine(GameObject enemy)
    {
        float time = 0f;
        float distance = Vector3.Distance(pullTransform.position, enemy.transform.position);
        
        while (time <= 0.2f)
        {
            time += Time.deltaTime;

            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, pullTransform.position, distance * 5f * Time.deltaTime);

            yield return null;
        }
    }
}
