using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 입력 제어 컴포넌트
public class PlayerInputController : MonoBehaviour
{
    Animator animator;
    InputMeleeAttack attackInput;
    InputNavmeshMovement movement;
    PlayerHealth health;
    PlayerSkillResource skillResource;
    CombatUIManager combatUIManager;

    // 이동 관련
    [SerializeField] private bool isRotationLocked;     // 스킬 발동 중 회전이 가능한지 여부
    [SerializeField] private bool isMovementLocked;     // 스킬 발동 중 이동이 가능한지 여부
    [SerializeField] private LayerMask moveLayerMask;   // 이동 레이어마스크
    [SerializeField] private LayerMask attackLayerMask; // 공격 레이어마스크

    // 스킬 관련
    private Coroutine[] cooldownCoroutines = new Coroutine[7];          // 쿨타임 계산 코루틴
    private WaitForSeconds[] waitForSeconds = new WaitForSeconds[7];    // 쿨타임 시간

    private Collider interactCollider = null;   // 상호작용이 가능한 콜라이더
    private bool isMouseLock = false;           // 적 클릭 시 잠시 마우스 입력을 막음
    private WaitForSeconds mouseWait;           // 마우스 입력 지연 시간

    private bool isDialogue = false;            // 다이얼로그 대화 상태인지
    private int dialogueStageIndex = 0;         // 현재 다이얼로그 진행 인덱스

    public bool IsRotationLocked { get => isRotationLocked; set => isRotationLocked = value; }
    public bool IsMovementLocked { get => isMovementLocked; set => isMovementLocked = value; }
    public Collider InteractCollider { get => interactCollider; set => interactCollider = value; }
    public bool IsDialogue { get => isDialogue; set => isDialogue = value; }

    public delegate void PlayerPotionDelegate();
    public static PlayerPotionDelegate playerPotionDelegate;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attackInput = GetComponent<InputMeleeAttack>();
        movement = GetComponent<InputNavmeshMovement>();
        health = GetComponent<PlayerHealth>();
        skillResource = GetComponent<PlayerSkillResource>();

        mouseWait = new WaitForSeconds(0.1f);
    }

    private void Start()
    {
        combatUIManager = FindAnyObjectByType<CombatUIManager>();

        for (int i = 0; i < waitForSeconds.Length; i++)
        {
            waitForSeconds[i] = new WaitForSeconds(combatUIManager.PlayerSkills[i].Data.skillCooldownTime);
        }
    }

    private void OnEnable()
    {
        PlayerHealth.playerReviveDelegate += InitCooldown;
        DialoguePopup.dialogueEndDelegate += OnDialogueEnd;
        DungeonEventManager.bossRoomClear += OnBossClear;
    }

    private void OnDisable()
    {
        PlayerHealth.playerReviveDelegate -= InitCooldown;
        DialoguePopup.dialogueEndDelegate -= OnDialogueEnd;
        DungeonEventManager.bossRoomClear -= OnBossClear;
    }

    private void Update()
    {
        // 플레이어 사망 시 스페이스 바를 눌러 부활
        if (!health.IsAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerHealth.playerReviveDelegate();
            }

            return;
        }

        // 상호작용
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (interactCollider != null)
            {
                InteractableObject interactable = interactCollider.GetComponent<InteractableObject>();
                
                if (interactable != null)
                {
                    movement.Agent.ResetPath();

                    Vector3 direction = (interactCollider.transform.position - transform.position).normalized;
                    transform.rotation = Quaternion.LookRotation(direction);

                    InteractableNPC npc = interactable.GetComponent<InteractableNPC>();
                    if (npc != null)
                    {
                        npc.DialogueStageIndex = dialogueStageIndex;
                    }

                    interactable.Interaction();
                }

                return;
            }
        }

        // 대화 중일 경우 리턴
        if (IsDialogue) return;

        // 회복 포션 사용
        if (Input.GetKeyDown(KeyCode.F))
        {
            health.UseHpPotion();
            playerPotionDelegate();
        }

        // 마우스 왼쪽 클릭했을 경우
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackLayerMask))
            {
                if (hit.collider.tag.Equals("Enemy"))
                {
                    attackInput.TouchInputAttack(hit.collider);
                    StartCoroutine(MouseLockCoroutine());
                    return;
                }
            }
        }
        else if (Input.GetMouseButton(0) && !isMouseLock)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, moveLayerMask))
            {
                movement.SetAgentDestination(hit.point);
                animator.ResetTrigger(StringToHash.MeleeAttack);
                attackInput.TargetCollider = null;
            }
        }

        // 공격 중일 경우 리턴
        if (attackInput.IsPlayAttackAnimation()) return;

        // 스킬 사용 중일 경우 리턴
        if (IsAction())
        {
            attackInput.TargetCollider = null;
            return;
        }

        // 스킬 사용
        if (Input.GetKey(KeyCode.Q))
        {
            ActivateSkill(0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            ActivateSkill(1);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            ActivateSkill(2);
        }
        else if (Input.GetKey(KeyCode.R))
        {
            ActivateSkill(3);
        }
        else if (Input.GetMouseButton(0))
        {
            ActivateSkill(4);
        }
        else if (Input.GetMouseButton(1))
        {
            ActivateSkill(5);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            ActivateSkill(6);
        }
    }

    private bool IsAction()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            return false;

        return true;
    }

    // 스킬 발동
    private void ActivateSkill(int index)
    {
        if (combatUIManager.IsSkillCooldown[index]) return;

        if (skillResource.CurrentResource < combatUIManager.PlayerSkills[index].Data.skillResource) return;

        combatUIManager.PlayerSkills[index].PerformSkill();
        combatUIManager.SkillActivated(index);
        cooldownCoroutines[index] = StartCoroutine(CooldownCoroutine(index));
        skillResource.CurrentResource -= combatUIManager.PlayerSkills[index].Data.skillResource;
        PlayerSkillResource.playerResourceDelegate();
    }

    // 쿨타임 계산 코루틴
    private IEnumerator CooldownCoroutine(int index)
    {
        combatUIManager.IsSkillCooldown[index] = true;
        yield return waitForSeconds[index];
        combatUIManager.IsSkillCooldown[index] = false;
    }

    // 부활 시 진행중이던 쿨타임 계산 초기화
    public void InitCooldown()
    {
        for (int i = 0; i < cooldownCoroutines.Length; i++)
        {
            if (cooldownCoroutines[i] != null)
            {
                StopCoroutine(cooldownCoroutines[i]);
                combatUIManager.IsSkillCooldown[i] = false;
                combatUIManager.InitCombatUI(i);

                if (combatUIManager.PlayerSkills[i].TYPE == Skill.SKILLTYPE.BUFF)
                    combatUIManager.PlayerSkills[i].DeActivateSkill();
            }
        }
    }

    // 마우스 클릭 지연 코루틴
    private IEnumerator MouseLockCoroutine()
    {
        isMouseLock = true;
        yield return mouseWait;
        isMouseLock = false;
    }

    // 다이얼로그 대화가 끝날 시 대화 인덱스 변경
    private void OnDialogueEnd()
    {
        IsDialogue = false;

        if (dialogueStageIndex == 0)
            dialogueStageIndex = 1;
        else if (dialogueStageIndex == 2)
            dialogueStageIndex = 3;
        else if (dialogueStageIndex == 3)
            StartCoroutine(LoadEndSceneCoroutine());
    }

    // 보스 클리어 시 대화 인덱스 변경
    private void OnBossClear()
    {
        dialogueStageIndex = 2;
    }

    private IEnumerator LoadEndSceneCoroutine()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");
    }
}
