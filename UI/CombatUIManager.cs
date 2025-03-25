using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 전투 UI 관리 컴포넌트
public class CombatUIManager : MonoBehaviour
{
    private PlayerHealth health;
    private PlayerSkillResource resource;

    [SerializeField] private GameObject player;         // 플레이어 참조
    [SerializeField] private Image hpValueImage;        // HP 이미지
    [SerializeField] private Image mpValueImage;        // MP 이미지

    [SerializeField] private Text maxPotionCount;       // 최대 포션 갯수
    [SerializeField] private Text currentPotionCount;   // 현재 포션 갯수

    [SerializeField] private SkillSlotUI[] skillSlots = new SkillSlotUI[7]; // 스킬 슬롯 컴포넌트 참조 배열

    // 스킬 관련
    [SerializeField] private Skill[] playerSkills;      // 각 스킬 슬롯의 스킬 정보 참조 배열
    private bool[] isSkillCooldown = new bool[7] { false, false, false, false, false, false, false };

    public SkillSlotUI[] SkillSlots { get => skillSlots; set => skillSlots = value; }
    public Skill[] PlayerSkills { get => playerSkills; set => playerSkills = value; }
    public bool[] IsSkillCooldown { get => isSkillCooldown; set => isSkillCooldown = value; }

    private void OnEnable()
    {
        PlayerHealth.playerHpDelegate += HpUIUpdate;
        PlayerInputController.playerPotionDelegate += PotionCountUpdate;
        PlayerSkillResource.playerResourceDelegate += MpUIUpdate;
    }

    private void OnDisable()
    {
        PlayerHealth.playerHpDelegate -= HpUIUpdate;
        PlayerInputController.playerPotionDelegate -= PotionCountUpdate;
        PlayerSkillResource.playerResourceDelegate -= MpUIUpdate;
    }

    private void Awake()
    {
        health = player.GetComponent<PlayerHealth>();
        resource = player.GetComponent<PlayerSkillResource>();
    }

    private void Start()
    {       
        for (int i = 0; i < SkillSlots.Length; i++)
        {
            SkillSlots[i].Init(this);
            skillSlots[i].Skill = playerSkills[i];
            skillSlots[i].Show();
        }
    }

    // 스킬 슬롯 초기화
    public void InitCombatUI(int index)
    {
        skillSlots[index].Init(this);
    }

    // 스킬이 사용 되었을 경우 쿨타임 표시
    public void SkillActivated(int index)
    {
        SkillSlots[index].ShowSkillCooldown();
    }

    // HP 업데이트 콜백
    public void HpUIUpdate()
    {
        hpValueImage.fillAmount = (float)health.CurrentHp / health.MaxHp;
    }

    // MP 업데이트 콜백
    public void MpUIUpdate()
    {
        mpValueImage.fillAmount = (float)resource.CurrentResource / resource.MaxResource;
    }

    // 포션 갯수 업데이트 콜백
    public void PotionCountUpdate()
    {
        currentPotionCount.text = health.CurrentPotionCount.ToString();
        maxPotionCount.text = health.MaxPotionCount.ToString();
    }
}
