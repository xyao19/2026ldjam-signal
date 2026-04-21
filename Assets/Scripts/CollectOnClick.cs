using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CollectOnClick : MonoBehaviour
{
    [Header("玩家Tag")]
    [SerializeField] private string playerTag = "Player";

    [Header("收集后显示的UI")]
    [SerializeField] private GameObject collectedUIRoot;

    private Camera mainCam;
    private Collider2D col;

    private bool canInteract = false;
    private bool isCollected = false;

    private void Awake()
    {
        mainCam = Camera.main;
        col = GetComponent<Collider2D>();

        if (collectedUIRoot != null)
        {
            collectedUIRoot.SetActive(false);
        }
    }

    private void Update()
    {
        if (isCollected || !canInteract) return;
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.collider == col)
            {
                Collect();
            }
        }
    }

    private void Collect()
    {
        isCollected = true;
        canInteract = false;

        if (collectedUIRoot != null)
        {
            collectedUIRoot.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            canInteract = false;
        }
    }
}
