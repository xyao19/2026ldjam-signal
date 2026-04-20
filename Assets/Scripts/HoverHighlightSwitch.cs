using UnityEngine;

public class HoverHighlightSwitch : MonoBehaviour
{
    [Header("Sprite …Ë÷√")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite outlineSprite;

    [Header("ÕÊº“Tag")]
    [SerializeField] private string playerTag = "Player";

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null && normalSprite != null)
        {
            sr.sprite = normalSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (sr != null && outlineSprite != null)
            {
                sr.sprite = outlineSprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (sr != null && normalSprite != null)
            {
                sr.sprite = normalSprite;
            }
        }
    }
}
