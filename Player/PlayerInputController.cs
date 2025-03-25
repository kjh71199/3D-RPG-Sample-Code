using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾� �Է� ���� ������Ʈ
public class PlayerInputController : MonoBehaviour
{
    Animator animator;
    InputMeleeAttack attackInput;
    InputNavmeshMovement movement;
    PlayerHealth health;
    PlayerSkillResource skillResource;
    CombatUIManager combatUIManager;

    // �̵� ����
    [SerializeField] private bool isRotationLocked;     // ��ų �ߵ� �� ȸ���� �������� ����
    [SerializeField] private bool isMovementLocked;     // ��ų �ߵ� �� �̵��� �������� ����
    [SerializeField] private LayerMask moveLayerMask;   // �̵� ���̾��ũ
    [SerializeField] private LayerMask attackLayerMask; // ���� ���̾��ũ

    // ��ų ����
    private Coroutine[] cooldownCoroutines = new Coroutine[7];          // ��Ÿ�� ��� �ڷ�ƾ
    private WaitForSeconds[] waitForSeconds = new WaitForSeconds[7];    // ��Ÿ�� �ð�

    private Collider interactCollider = null;   // ��ȣ�ۿ��� ������ �ݶ��̴�
    private bool isMouseLock = false;           // �� Ŭ�� �� ��� ���콺 �Է��� ����
    private WaitForSeconds mouseWait;           // ���콺 �Է� ���� �ð�

    private bool isDialogue = false;            // ���̾�α� ��ȭ ��������
    private int dialogueStageIndex = 0;         // ���� ���̾�α� ���� �ε���

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
        // �÷��̾� ��� �� �����̽� �ٸ� ���� ��Ȱ
        if (!health.IsAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerHealth.playerReviveDelegate();
            }

            return;
        }

        // ��ȣ�ۿ�
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

        // ��ȭ ���� ��� ����
        if (IsDialogue) return;

        // ȸ�� ���� ���
        if (Input.GetKeyDown(KeyCode.F))
        {
            health.UseHpPotion();
            playerPotionDelegate();
        }

        // ���콺 ���� Ŭ������ ���
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

        // ���� ���� ��� ����
        if (attackInput.IsPlayAttackAnimation()) return;

        // ��ų ��� ���� ��� ����
        if (IsAction())
        {
            attackInput.TargetCollider = null;
            return;
        }

        // ��ų ���
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

    // ��ų �ߵ�
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

    // ��Ÿ�� ��� �ڷ�ƾ
    private IEnumerator CooldownCoroutine(int index)
    {
        combatUIManager.IsSkillCooldown[index] = true;
        yield return waitForSeconds[index];
        combatUIManager.IsSkillCooldown[index] = false;
    }

    // ��Ȱ �� �������̴� ��Ÿ�� ��� �ʱ�ȭ
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

    // ���콺 Ŭ�� ���� �ڷ�ƾ
    private IEnumerator MouseLockCoroutine()
    {
        isMouseLock = true;
        yield return mouseWait;
        isMouseLock = false;
    }

    // ���̾�α� ��ȭ�� ���� �� ��ȭ �ε��� ����
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

    // ���� Ŭ���� �� ��ȭ �ε��� ����
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
