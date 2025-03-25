using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : InteractableObject
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private int dialogueStageIndex;
    [SerializeField] private int dialogueIndex;
    [SerializeField] private Collider cryptEnterCollider;

    private GameObject player;
    private bool isInteraction = false;

    public int DialogueStageIndex { get => dialogueStageIndex; set => dialogueStageIndex = value; }

    private void OnEnable()
    {
        DialoguePopup.dialogueEndDelegate += OnDialogueEnd;
    }

    private void OnDisable()
    {
        DialoguePopup.dialogueEndDelegate -= OnDialogueEnd;
    }

    public override void Interaction()
    {
        if (!isInteraction)
        {
            image.gameObject.SetActive(false);
            dialogueManager.LoadDialogue(DialogueStageIndex, dialogueIndex);
            isInteraction = true;

            if (player != null)
            {
                player.GetComponent<PlayerInputController>().IsDialogue = true;
            }
        }
        else
        {
            dialogueManager.LoadNextDialogue();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        player = other.gameObject;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        player = null;
    }

    private void OnDialogueEnd()
    {
        isInteraction = false;

        if (DialogueStageIndex == 0)
            cryptEnterCollider.enabled = true;
    }
}
