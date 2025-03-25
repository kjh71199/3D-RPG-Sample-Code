using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 마법 구체 투사체 컴포넌트
public class SkeletonMagicBall : SkeletonProjectile
{
    private MonsterMagicAttackState magicAttack;

    private void Awake()
    {
        magicAttack = GetComponent<MonsterMagicAttackState>();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveDistanceCoroutine());
    }

    private IEnumerator MoveDistanceCoroutine()
    {
        start = transform.position;
        float distance = 0f;

        while (distance <= maxDistance)
        {
            distance = Vector3.Distance(start, transform.position);
            transform.Translate(Direction * speed * Time.deltaTime, Space.World);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
