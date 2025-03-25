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

    // 다이얼로그 팝업 클리어
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

    // 다이얼로그에 대화내용 출력
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

    // 타이핑 효과 코루틴
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

    // 타이핑 스킵
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

    // 다음 대화로 이동
    public void OnNextButtonClick()
    {
        if (IsTyping) return;

        manager.Next();
    }

    // 대화 종료
    public void OnFinishButtonClick()
    {
        gameObject.SetActive(false);
        dialogueEndDelegate();
    }
}
