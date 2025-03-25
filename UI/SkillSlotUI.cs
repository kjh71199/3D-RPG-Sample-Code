using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 스킬 슬롯 UI 컴포넌트
public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image skillIcon;           // 스킬 아이콘 이미지
    [SerializeField] private Image skillCoolDownImage;  // 스킬 쿨타임 표시 이미지

    [SerializeField] private CombatUIManager skillUIManager;

    [SerializeField] private Skill skill;   // 해당 슬롯의 스킬
    private PlayerSkillSO skillSO;          // 해당 슬롯의 스킬 정보 스크립터블 오브젝트
    private Coroutine coolDownCoroutine;    // 해당 슬롯의 스킬 쿨타임 표시 코루틴

    public Skill Skill { get => skill; set => skill = value; }

    // 스킬 슬롯 초기화
    public void Init(CombatUIManager skillUIManager)
    {
        this.skillUIManager = skillUIManager;

        if (coolDownCoroutine != null)
            StopCoroutine(coolDownCoroutine);

        skillCoolDownImage.fillAmount = 0f;
    }

    // 스킬 아이콘 표시
    public void Show()
    {
        skillSO = skill.Data;

        skillIcon.sprite = skillSO.skillSprite;
    }

    // 스킬 아이콘 비우기
    public void Clear()
    {
        Skill = null;
        skillIcon.sprite = null;
    }

    // 스킬 쿨타임 표시
    public void ShowSkillCooldown()
    {
        if (coolDownCoroutine !=  null)
            StopCoroutine(coolDownCoroutine);

        coolDownCoroutine = StartCoroutine(SkillCooldown());
    }

    // 스킬 쿨타임 코루틴
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
