using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : InteractableObject
{
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    public override void Interaction()
    {
        animator.SetTrigger("Open");
        SoundManager.Instance.PlayDungeonSound(SoundManager.DUNGEONSOUND.DOOR);
    }

}
