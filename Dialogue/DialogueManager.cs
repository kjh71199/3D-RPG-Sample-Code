using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueDatabase dialogueDatabase;
    [SerializeField] private DialoguePopup dialoguePopup;

    private int currentDialogueIndex;
    private DialogueDatas currentDialogueDatas;
    private int lastDialogue;

    public void LoadDialogue(int stageIndex, int dialogueIndex)
    {
        currentDialogueIndex = dialogueIndex;

        currentDialogueDatas = dialogueDatabase.List[stageIndex];

        LoadDialogueData(currentDialogueIndex);
    }

    public void LoadNextDialogue()
    {
        if (dialoguePopup.IsTyping)
        {
            DialogueData dialogueData = currentDialogueDatas.List[currentDialogueIndex];
            string dialogueLine = dialogueData.dialogueLine.Replace("\\n", "\n");
            bool isLast = currentDialogueIndex >= currentDialogueDatas.List.Length - 1;

            dialoguePopup.TypeSkip(dialogueLine, isLast);
            return;
        }

        if (currentDialogueIndex >= lastDialogue)
        {
            dialoguePopup.OnFinishButtonClick();
            return;
        }

        dialoguePopup.OnNextButtonClick();
    }

    public void UnLoadDialogue()
    {
        currentDialogueIndex = 0;
        dialoguePopup.Hide();
    }

    private void LoadDialogueData(int currentDialogueIndex)
    {
        DialogueData dialogueData = currentDialogueDatas.List[currentDialogueIndex];

        DIR direction = dialogueData.direction;

        int nameId = dialogueData.nameId;
        string name = currentDialogueDatas.CharacterNames[nameId];

        string dialogueLine = dialogueData.dialogueLine.Replace("\\n", "\n");

        bool isLast = currentDialogueIndex >= currentDialogueDatas.List.Length - 1;
        lastDialogue = currentDialogueDatas.List.Length - 1;

        if (!dialoguePopup.IsTyping)
            dialoguePopup.Show(this, direction, name, dialogueLine, isLast);
    }

    public void Next()
    {
        currentDialogueIndex++;
        dialoguePopup.IsTypingEnd = false;
        LoadDialogueData(currentDialogueIndex);
    }
}
