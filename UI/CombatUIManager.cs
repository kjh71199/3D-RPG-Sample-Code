using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �÷��̾� ���� UI ���� ������Ʈ
public class CombatUIManager : MonoBehaviour
{
    private PlayerHealth health;
    private PlayerSkillResource resource;

    [SerializeField] private GameObject player;         // �÷��̾� ����
    [SerializeField] private Image hpValueImage;        // HP �̹���
    [SerializeField] private Image mpValueImage;        // MP �̹���

    [SerializeField] private Text maxPotionCount;       // �ִ� ���� ����
    [SerializeField] private Text currentPotionCount;   // ���� ���� ����

    [SerializeField] private SkillSlotUI[] skillSlots = new SkillSlotUI[7]; // ��ų ���� ������Ʈ ���� �迭

    // ��ų ����
    [SerializeField] private Skill[] playerSkills;      // �� ��ų ������ ��ų ���� ���� �迭
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

    // ��ų ���� �ʱ�ȭ
    public void InitCombatUI(int index)
    {
        skillSlots[index].Init(this);
    }

    // ��ų�� ��� �Ǿ��� ��� ��Ÿ�� ǥ��
    public void SkillActivated(int index)
    {
        SkillSlots[index].ShowSkillCooldown();
    }

    // HP ������Ʈ �ݹ�
    public void HpUIUpdate()
    {
        hpValueImage.fillAmount = (float)health.CurrentHp / health.MaxHp;
    }

    // MP ������Ʈ �ݹ�
    public void MpUIUpdate()
    {
        mpValueImage.fillAmount = (float)resource.CurrentResource / resource.MaxResource;
    }

    // ���� ���� ������Ʈ �ݹ�
    public void PotionCountUpdate()
    {
        currentPotionCount.text = health.CurrentPotionCount.ToString();
        maxPotionCount.text = health.MaxPotionCount.ToString();
    }
}
