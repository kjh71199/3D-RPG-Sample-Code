using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��ų ���� UI ������Ʈ
public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image skillIcon;           // ��ų ������ �̹���
    [SerializeField] private Image skillCoolDownImage;  // ��ų ��Ÿ�� ǥ�� �̹���

    [SerializeField] private CombatUIManager skillUIManager;

    [SerializeField] private Skill skill;   // �ش� ������ ��ų
    private PlayerSkillSO skillSO;          // �ش� ������ ��ų ���� ��ũ���ͺ� ������Ʈ
    private Coroutine coolDownCoroutine;    // �ش� ������ ��ų ��Ÿ�� ǥ�� �ڷ�ƾ

    public Skill Skill { get => skill; set => skill = value; }

    // ��ų ���� �ʱ�ȭ
    public void Init(CombatUIManager skillUIManager)
    {
        this.skillUIManager = skillUIManager;

        if (coolDownCoroutine != null)
            StopCoroutine(coolDownCoroutine);

        skillCoolDownImage.fillAmount = 0f;
    }

    // ��ų ������ ǥ��
    public void Show()
    {
        skillSO = skill.Data;

        skillIcon.sprite = skillSO.skillSprite;
    }

    // ��ų ������ ����
    public void Clear()
    {
        Skill = null;
        skillIcon.sprite = null;
    }

    // ��ų ��Ÿ�� ǥ��
    public void ShowSkillCooldown()
    {
        if (coolDownCoroutine !=  null)
            StopCoroutine(coolDownCoroutine);

        coolDownCoroutine = StartCoroutine(SkillCooldown());
    }

    // ��ų ��Ÿ�� �ڷ�ƾ
    private IEnumerator SkillCooldown()
    {
        float time = skillSO.skillCooldownTime;
        skillCoolDownImage.fillAmount = 1f;

        while (time >= 0)
        {
            time -= Time.deltaTime;

            skillCoolDownImage.fillAmount = time / skillSO.skillCooldownTime;

            yield return null;
        }
    }
}
