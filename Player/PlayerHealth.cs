using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    protected Animator animator;
    protected Stats stats;
    protected InputNavmeshMovement movement;

    private int hashHit = Animator.StringToHash("Hit");

    private AnimatorStateInfo currentStateInfo;

    [Header("Hit")]
    [SerializeField] protected float knockbackTime;
    [SerializeField] protected float detroyDelayTime;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected ParticleSystem hitParticle;

    [Header("Potion")]
    [SerializeField] protected int maxPotionCount;
    [SerializeField] protected int currentPotionCount;
    [SerializeField] protected int potionRegenAmount;

    public delegate void PlayerHpDelegate();
    public static PlayerHpDelegate playerHpDelegate;

    public delegate void PlayerDieDelegate();
    public static PlayerDieDelegate playerDieDelegate;

    public delegate void PlayerReviveDelegate();
    public static PlayerReviveDelegate playerReviveDelegate;

    public int MaxPotionCount { get => maxPotionCount; set => maxPotionCount = value; }
    public int CurrentPotionCount { get => currentPotionCount; set => currentPotionCount = value; }

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        movement = GetComponent<InputNavmeshMovement>();
    }

    protected void Start()
    {
        MaxHp = stats.maxHp;
        CurrentHp = MaxHp;

        StartCoroutine(HpRegenCoroutine());

        InitPotion();
    }

    private void OnEnable()
    {
        playerReviveDelegate += Revive;
    }

    private void OnDisable()
    {
        playerReviveDelegate -= Revive;
    }

    public override void Hit(float damage, float knockback, Vector3 direction, Enums.CROWDCONTROL control)
    {
        hitParticle.Play();
        
        CurrentHp -= (int)damage;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHp);
        playerHpDelegate();

        if (CurrentHp <= 0)
        {
            Die();
            return;
        }
    }

    protected void Die()
    {
        IsAlive = false;
        animator.SetBool("Die", true);
        playerDieDelegate();
    }

    private void Revive()
    {
        LoadingSceneManager.LoadNextScene("DungeonScene", new Vector3(2.5f, 0f, -2.5f), new Vector3(1f, 0f, 1f));
        animator.SetBool("Die", false);

        IsAlive = true;

        StartCoroutine(ReviveCoroutine());
    }

    private IEnumerator ReviveCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        InitPlayerHp();
        InitPotion();
    }

    private void InitPlayerHp()
    {
        CurrentHp = stats.maxHp;
        playerHpDelegate();
    }

    public bool IsPlayHitAnimation()
    {
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (currentStateInfo.shortNameHash == hashHit)
            return true;

        return false;
    }

    private IEnumerator ApplyHitKnockback(Vector3 hitDirection, float force)
    {
        float timer = 0f;
        while (timer < knockbackTime)
        {
            RaycastHit ray;
            if (Physics.Raycast(transform.position + new Vector3(0f, 1f, 0f), hitDirection, out ray, 0.7f, wallLayer))
            {
                force = 0f;
            }

            transform.Translate(hitDirection * force * Time.fixedDeltaTime, Space.World);
            movement.MovePosition = transform.position;

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual IEnumerator HpRegenCoroutine()
    {
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;

            if (CurrentHp <= 0)
                break;

            if (time >= 1f)
            {
                if (CurrentHp < MaxHp)
                {
                    CurrentHp += stats.hpRegen;
                    playerHpDelegate();
                }

                time = 0f;
            }
            yield return null;
        }
    }

    public void UseHpPotion()
    {
        if (CurrentPotionCount > 0 && CurrentHp < MaxHp)
        {
            CurrentHp += potionRegenAmount;
            CurrentPotionCount--;
            playerHpDelegate();
        }
    }

    private void InitPotion()
    {
        CurrentPotionCount = MaxPotionCount;
        PlayerInputController.playerPotionDelegate();
    }
}
