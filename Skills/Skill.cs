using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public enum SKILLTYPE { ATTACK, BUFF }

    protected Animator animator;
    protected Stats stats;
    protected InputNavmeshMovement movement;
    protected PlayerInputController controller;
    protected SoundFx soundFx;

    protected GameObject effect;
    protected ParticleSystem effectParticle;

    [SerializeField] protected PlayerSkillSO data;
    [SerializeField] protected Transform skillEffectTransform;
    [SerializeField] protected Vector3 skillEffectOffset;
    [SerializeField] protected SKILLTYPE type;

    public PlayerSkillSO Data { get => data; set => data = value; }
    public SKILLTYPE TYPE { get => type; set => type = value; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        movement = GetComponent<InputNavmeshMovement>();
        controller = GetComponent<PlayerInputController>();
        soundFx = GetComponent<SoundFx>();

        if (data.skillEffect != null)
        {
            effect = Instantiate(data.skillEffect);
            effect.transform.SetParent(skillEffectTransform);
            effect.transform.position = transform.position + skillEffectOffset;
            effectParticle = effect.GetComponent<ParticleSystem>();
        }
    }

    public abstract void PerformSkill();

    public virtual void DeActivateSkill()
    {

    }
}
