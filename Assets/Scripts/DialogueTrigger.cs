using UnityEngine;
using NodeCanvas.DialogueTrees;

public class DialogueTrigger : MonoBehaviour
{
    [Header("绑定你的对话控制器")]
    public DialogueTreeController dialogueTree;

    [Header("是否只触发一次")]
    public bool onlyOnce = true;

    private bool hasTriggered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            dialogueTree.StartDialogue();

            if (onlyOnce)
                hasTriggered = true;
        }
    }
}