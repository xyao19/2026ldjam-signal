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

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 moveInput;
    private bool isGrounded;
    private Vector3 visualOriginalScale;

    private bool isWalking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (visual != null)
        {
            animator = visual.GetComponent<Animator>();
            visualOriginalScale = visual.localScale;
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.velocity.y);
        }

        if (visual != null)
        {
            if (moveInput.x > 0.01f)
            {
                visual.localScale = new Vector3(
                    Mathf.Abs(visualOriginalScale.x),
                    visualOriginalScale.y,
                    visualOriginalScale.z
                );
            }
            else if (moveInput.x < -0.01f)
            {
                visual.localScale = new Vector3(
                    -Mathf.Abs(visualOriginalScale.x),
                    visualOriginalScale.y,
                    visualOriginalScale.z
                );
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }

    // 给 Move 事件调用
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // 只根据左右输入决定是否走路
        isWalking = Mathf.Abs(moveInput.x) > 0.01f;

        if (visual != null)
        {
            if (moveInput.x > 0.01f)
            {
                visual.localScale = new Vector3(
                    Mathf.Abs(visualOriginalScale.x),
                    visualOriginalScale.y,
                    visualOriginalScale.z
                );
            }
            else if (moveInput.x < -0.01f)
            {
                visual.localScale = new Vector3(
                    -Mathf.Abs(visualOriginalScale.x),
                    visualOriginalScale.y,
                    visualOriginalScale.z
                );
            }
        }
    }

    // 给 Jump 事件调用
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