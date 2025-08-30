using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TextLine
{
    public string speakerName;
    [TextArea(2, 3)]
    public string dialogueLine;
}

[CreateAssetMenu(fileName = "NewConversation", menuName = "Dialogue/Conversation")]
public class ConversationTemplate : ScriptableObject
{
    public List<TextLine> conversationLines;
}
