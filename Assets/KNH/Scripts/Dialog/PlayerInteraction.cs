using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private Text nameText;
    [SerializeField] private Text dialogText;
    [SerializeField] NPCInfo npcInfo;

    [SerializeField] DialogueEvent dialogue;
    [SerializeField] private float typingSpeed = 0.05f; // 한 글자 출력 간격
    Dialogue[] dialogues;
    int lineCount = 0;//대화 카운트
    int contextCount = 0; //대사 카운트
    int npcID;
    private bool isTyping = false;

    private Player player;

    private int index = 0;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NPCInfo>() != null)
        {
            npcInfo = collision.GetComponent<NPCInfo>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<NPCInfo>() != null)
        {
            player.isDialog = false;
        }
    }

    private void Update()
    {

        if (!player.IsNearBench() && !dialogUI.activeSelf && Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 대화 시작
            player.isDialog = true;
            dialogUI.SetActive(true);
            ShowDialogue(GetDialogue(npcInfo.npcID));
        }
        else if (dialogUI.activeSelf && Input.GetKeyDown(KeyCode.Z))
        {
            if (isTyping)
            {
                // 현재 타이핑 중이면 전체 문장 즉시 출력
                StopAllCoroutines();
                dialogText.text = dialogues[lineCount].contexts[contextCount];
                isTyping = false;
            }
            else
            {
                // 다음 대사 출력
                ShowNextDialogue();
            }
        }
    }

    private void ShowNextDialogue()
    {
        contextCount++;

        if (contextCount >= dialogues[lineCount].contexts.Length)
        {
            lineCount++;
            contextCount = 0;
        }

        if (lineCount >= dialogues.Length)
        {
            // 모든 대사 종료 → UI 끄고 변수 초기화
            dialogUI.SetActive(false);
            lineCount = 0;
            contextCount = 0;
            player.isDialog = false;
            return;
        }

        nameText.text = dialogues[lineCount].name;
        StartCoroutine(TypeWriter());
    }

    public Dialogue[] GetDialogue(int id)
    {
        dialogue.dialogues = DatabaseManager.instance.GetDialogue(id);
        return dialogue.dialogues;
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        dialogText.text = "";
        nameText.text = "";//초기화

        dialogues = p_dialogues;

        index = 0;
        nameText.text = dialogues[lineCount].name;
        StartCoroutine("TypeWriter");
    }
    private IEnumerator TypeWriter()
    {
        isTyping = true;
        dialogText.text = "";

        string targetText = dialogues[lineCount].contexts[contextCount];

        foreach (char letter in targetText)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

}