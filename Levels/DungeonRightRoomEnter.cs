using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ �� ���� �̺�Ʈ
public class DungeonRightRoomEnter : MonoBehaviour
{
    private Collider rightCollider;

    private void Awake()
    {
        rightCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("RightEnter");
            rightCollider.enabled = false;
            DungeonEventManager.rightRoomEnter();
        }
    }
}
