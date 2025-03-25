using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 플레이어 스킬 자원 컴포넌트
public class PlayerSkillResource : MonoBehaviour
{
    [SerializeField] protected Stats stats;

    private float maxResource;
    [SerializeField] private float currentResource;

    public float MaxResource { get => maxResource; set => maxResource = value; }
    public float CurrentResource
    {
        get => currentResource;
        set
        {
            currentResource = value;

            if (currentResource > maxResource)
                currentResource = maxResource;
        }
    }

    public delegate void PlayerResourceDelegate();
    public static PlayerResourceDelegate playerResourceDelegate;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    protected void Start()
    {
        MaxResource = stats.maxMp;
        CurrentResource = MaxResource;

        StartCoroutine(ResourceRegenCoroutine());
    }

    private void OnEnable()
    {
        PlayerHealth.playerReviveDelegate += InitPlayerResource;
    }

    private void OnDisable()
    {
        PlayerHealth.playerReviveDelegate -= InitPlayerResource;
    }

    private void InitPlayerResource()
    {
        CurrentResource = MaxResource;
        playerResourceDelegate();
    }

    protected virtual IEnumerator ResourceRegenCoroutine()
    {
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;

            if (time >= 1f)
            {
                if (currentResource < MaxResource)
                {
                    currentResource += stats.mpRegen;
                    playerResourceDelegate();
                }

                time = 0f;
            }
            yield return null;
        }
    }
}
