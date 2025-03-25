using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 던전 왼쪽 방 입장 이벤트
public class DungeonLeftRoomEnter : MonoBehaviour
{
    private Collider leftCollider;

    private void Awake()
    {
        leftCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("LeftEnter");
            leftCollider.enabled = false;
            DungeonEventManager.leftRoomEnter();
        }
    }
}
