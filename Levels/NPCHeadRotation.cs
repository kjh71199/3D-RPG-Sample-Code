using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC의 머리 회전 컴포넌트
public class NPCHeadRotation : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private Animator animator;
    [SerializeField] private float angle;

    private Coroutine headCoroutine;

    private IEnumerator HeadCoroutine()
    {
        float time = 0f;
        while (time < 2f)
        {
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.LookRotation(transform.forward), time * 0.5f);

            time += Time.deltaTime;
            yield return null;
        }
        animator.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && headCoroutine != null)
            StopCoroutine(headCoroutine);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            animator.enabled = false;
            Vector3 directionToTarget = (other.transform.position - transform.position).normalized;

            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (angleToTarget < angle)
                head.transform.rotation = Quaternion.Slerp(head.transform.rotation, Quaternion.LookRotation(directionToTarget), 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            headCoroutine = StartCoroutine(HeadCoroutine());
        }
    }
}
