using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogoManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lineText;
    [SerializeField] GameObject dialoguePanel;

    Queue<TextLine> lines = new Queue<TextLine>();
    bool dialogueIsActive = false;
    Coroutine dialogueCoroutine;
    public ConversationTemplate conversationAsset;
    void Start()
    {
        StartConversation(conversationAsset);
    }
    public void StartConversation(ConversationTemplate conversation)
    {
        dialogueIsActive = true;
        dialoguePanel.SetActive(true);

        lines.Clear();
        foreach (var line in conversation.conversationLines)
        {
            lines.Enqueue(line);
        }

        if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(PlayConversation());
    }

    IEnumerator PlayConversation()
    {
        while (lines.Count > 0)
        {
            TextLine currentLine = lines.Dequeue();

            nameText.text = currentLine.speakerName;
            lineText.text = currentLine.dialogueLine;

            // tiempo de lectura según longitud del texto
            float readTime = Mathf.Max(2f, currentLine.dialogueLine.Length * 0.08f);
            yield return new WaitForSeconds(readTime);
        }

        EndConversation();
    }

    void EndConversation()
    {
        dialogueIsActive = false;
        dialoguePanel.SetActive(false);
        lineText.text = "";
        nameText.text = "";
    }
}
