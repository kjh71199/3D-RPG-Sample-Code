using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ÇÃ·¹ÀÌ¾î »ç¸Á UI ÄÄÆ÷³ÍÆ®
public class PlayerDiePanel : MonoBehaviour
{
    [SerializeField] private GameObject playerDie;

    private void OnEnable()
    {
        PlayerHealth.playerDieDelegate += OnPlayerDiePanel;
        PlayerHealth.playerReviveDelegate += OffPlayerDiePanel;
    }

    private void OnDisable()
    {
        PlayerHealth.playerDieDelegate -= OnPlayerDiePanel;
        PlayerHealth.playerReviveDelegate -= OffPlayerDiePanel;
    }

    private void OnPlayerDiePanel()
    {
        playerDie.SetActive(true);
    }

    private void OffPlayerDiePanel()
    {
        playerDie.SetActive(false);
    }
}
