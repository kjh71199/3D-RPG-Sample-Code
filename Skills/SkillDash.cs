using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDash : Skill
{
    BuffSkillSO buffData;

    [SerializeField] private LayerMask moveLayerMask;
    [SerializeField] private LayerMask wallLayerMask;

    protected override void Awake()
    {
        base.Awake();
        buffData = (BuffSkillSO)data;
    }

    public override void PerformSkill()
    {
        ActivateDash();
    }

    private void ActivateDash()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, moveLayerMask))
        {
            StartCoroutine(DashCoroutine(hit.point));
        }
    }

    private IEnumerator DashCoroutine(Vector3 mousePoint)
    {
        float time = 0f;
        Vector3 direction = (mousePoint - transform.position).normalized;
        Vector3 destination = Vector3.zero;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0f, 1f, 0f), direction, out hit, 10f, wallLayerMask))
            destination = new Vector3(hit.point.x, 0f, hit.point.z) - direction.normalized;
        else
            destination = transform.position + direction.normalized * 5f;

        movement.SetAgentDestination(destination);

        soundFx.PlaySound(buffData.skillSoundFx);

        while (time <= buffData.skillDurationTime)
        {
            time += Time.deltaTime;
            Vector3 lookDir = destination - transform.position;

            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
                transform.position = Vector3.MoveTowards(transform.position, destination, stats.moveSpeed * 2f * Time.deltaTime);
            }

            if (transform.position == destination) break;

            yield return null;
        }
    }
}
