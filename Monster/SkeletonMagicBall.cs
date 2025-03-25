using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
