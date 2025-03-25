using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스 방 입장 이벤트
public class DungeonBossRoomEnter : MonoBehaviour
{
    private Collider bossCollider;

    private void Awake()
    {
        bossCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("BossEnter");
            bossCollider.enabled = false;
            DungeonEventManager.bossRoomEnter();
        }
    }
}
