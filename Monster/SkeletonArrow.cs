using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 화살 투사체 컴포넌트
public class SkeletonArrow : SkeletonProjectile
{
    private MonsterRangeAttackState rangeAttack;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        rangeAttack = GetComponent<MonsterRangeAttackState>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveDistanceCoroutine());
    }

    private IEnumerator MoveDistanceCoroutine()
    {
        start = transform.position;
        float distance = 0f;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        trailRenderer.Clear();

        while (distance <= maxDistance)
        {
            distance = Vector3.Distance(start, transform.position);
            transform.Translate(Direction * speed * Time.deltaTime, Space.World);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
