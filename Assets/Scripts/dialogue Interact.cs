using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;

public class dialogueInteract : MonoBehaviour
{
    private DialogueTreeController dialogueTree;

    private void Start()
    {
        dialogueTree = GetComponent<DialogueTreeController>();
        if (dialogueTree == null)
        {
            Debug.LogError("DialogueTreeController 未找到！");
        }
    }

    // 👇 必须加 public，按钮才能看到这个方法
    public void StartDialogue()
    {
        if (dialogueTree != null)
        {
            dialogueTree.StartDialogue();
        }
    }
}