using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("移动参数")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("地面检测")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("视觉层")]
    [SerializeField] private Transform visual;

    [Header("脚步声")]
    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.4f;

    private float stepTimer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;   // ⭐ 新增

    private Vector2 moveInput;
    private bool isGrounded;
    private bool isWalking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (visual != null)
        {
            animator = visual.GetComponent<Animator>();
            spriteRenderer = visual.GetComponent<SpriteRenderer>();   // ⭐ 新增
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }

        // 脚步声逻辑
        if (isWalking && isGrounded)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }

    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0 || footstepAudio == null) return;

        int index = Random.Range(0, footstepClips.Length);
        footstepAudio.PlayOneShot(footstepClips[index]);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isWalking = Mathf.Abs(moveInput.x) > 0.01f;

        // ⭐ 用 SpriteRenderer.flipX 翻转
        if (spriteRenderer != null)
        {
            if (moveInput.x > 0.01f)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveInput.x < -0.01f)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}