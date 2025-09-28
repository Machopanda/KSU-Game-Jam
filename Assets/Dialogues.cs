using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Dialogues : MonoBehaviour
{
    [SerializeField] DialogueData dialogueData;
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] TextMeshProUGUI NPCTextDisplay;
                                              
    private int currentNodeIndex = 0; // Current node in the dialogue tree

    private void Start()
    {
        
    }
    public void StartConversation()
    {
        currentNodeIndex = 0;
        dialogueCanvas.SetActive(true);
        ShowCurrentNode();
    }

    void ShowCurrentNode()
    {
        if (currentNodeIndex < dialogueData.getDialogueNodes().Length)
        {
            DialogueNode node = dialogueData.getDialogueNodes()[currentNodeIndex];
            ShowDialogue(node.GetText());
        }
        else
        {
            HideDialogue();
        }
    }

    void ShowDialogue(string NPCText)
    {
        NPCTextDisplay.text = NPCText;
    }

    void HideDialogue() // TODO: We should not be handling this here
    {
        dialogueCanvas.SetActive(false);  // Hides the whole dialogue panel
    }
}
