using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePopup : MonoBehaviour
{
    [SerializeField] private GameObject[] characterNames;
    [SerializeField] private Text dialogueLineText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject endButton;
    [SerializeField] private float typingDelayTime;

    private bool isTyping;
    private bool isTypingEnd;

    private DialogueManager manager;
    private Coroutine typingCoroutine;
    private WaitForSeconds typingWaits;

    public delegate void DialogueEndDelegate();
    public static DialogueEndDelegate dialogueEndDelegate;

    public bool IsTyping { get => isTyping; set => isTyping = value; }
    public bool IsTypingEnd { get => isTypingEnd; set => isTypingEnd = value; }

    private void Awake()
    {
        typingWaits = new WaitForSeconds(typingDelayTime);
    }

    // ���̾�α� �˾� Ŭ����
    public void ClearDialoguePopup()
    {
        nextButton.SetActive(false);

        for (int i = 0; i < 2; i++)
        {
            characterNames[i].SetActive(false);
        }
    }

    public void Hide()
    {
        ClearDialoguePopup();
        gameObject.SetActive(false);
    }

    // ���̾�α׿� ��ȭ���� ���
    public void Show(DialogueManager manager, DIR direction, string name, string line, bool isLast = false)
    {
        this.manager = manager;
        
        gameObject.SetActive(true);
        IsTypingEnd = false;

        ClearDialoguePopup();

        int dirctionIndex = (int)direction;
        
        characterNames[dirctionIndex].SetActive(true);
        characterNames[dirctionIndex].GetComponentInChildren<Text>().text = name;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeDialogue(line, isLast));
    }

    // Ÿ���� ȿ�� �ڷ�ƾ
    private IEnumerator TypeDialogue(string dialogue, bool isLast)
    {
        IsTyping = true;

        dialogueLineText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueLineText.text += letter;
            yield return typingWaits;
        }

        nextButton.SetActive(!isLast);
        endButton.SetActive(isLast);

        IsTyping = false;
        IsTypingEnd = true;
    }

    // Ÿ���� ��ŵ
    public void TypeSkip(string dialogue, bool isLast)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        
        dialogueLineText.text = dialogue;

        nextButton.SetActive(!isLast);
        endButton.SetActive(isLast);

        IsTyping = false;
        IsTypingEnd = true;
    }

    // ���� ��ȭ�� �̵�
    public void OnNextButtonClick()
    {
        if (IsTyping) return;

        manager.Next();
    }

    // ��ȭ ����
    public void OnFinishButtonClick()
    {
        gameObject.SetActive(false);
        dialogueEndDelegate();
    }
}
