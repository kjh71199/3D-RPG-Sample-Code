using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 보스 UI 관리 컴포넌트
public class BossUIManager : MonoBehaviour
{
    [SerializeField] private GameObject bossUIPanel;
    [SerializeField] private Image bossHpValueImage;
    [SerializeField] private SkeletonGolemHealth bossHealth;

    public delegate void BossHpUpdateDelegate();
    public static BossHpUpdateDelegate bossHpUpdate;

    private void OnEnable()
    {
        DungeonEventManager.bossRoomEnter += EnableBossUI;
        DungeonEventManager.bossRoomClear += DisableBossUI;
        bossHpUpdate += UpdateBossHpUI;
    }

    private void OnDisable()
    {
        DungeonEventManager.bossRoomEnter -= EnableBossUI;
        DungeonEventManager.bossRoomClear -= DisableBossUI;
        bossHpUpdate -= UpdateBossHpUI;
    }

    private void EnableBossUI()
    {
        bossUIPanel.SetActive(true);
    }

    private void DisableBossUI()
    {
        bossUIPanel.SetActive(false);
    }

    private void UpdateBossHpUI()
    {
        bossHpValueImage.fillAmount = (float)bossHealth.CurrentHp / bossHealth.MaxHp;
    }
}
