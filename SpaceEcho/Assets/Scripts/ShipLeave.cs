using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipLeave : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] private Animator doorAnimator;   // 门的Animator
    [SerializeField] private Transform shipRoot;      // 整艘飞船根节点

    [Header("点击检测")]
    [SerializeField] private Collider2D clickableCollider; // 点击区域（飞船）

    [Header("门关闭设置")]
    [SerializeField] private string closeTriggerName = "CloseDoor";
    [SerializeField] private float doorCloseDelay = 1f;

    [Header("飞船起飞设置")]
    [SerializeField] private Vector2 flyDirection = new Vector2(1f, 1f);
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private float flyDuration = 2f;

    private bool isFlying = false;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (isFlying) return;
        if (Mouse.current == null) return;

        // 鼠标点击检测
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.collider == clickableCollider)
            {
                PlayTakeOffSequence();
            }
        }
    }

    public void PlayTakeOffSequence()
    {
        if (!isFlying)
        {
            StartCoroutine(TakeOffSequence());
        }
    }

    private IEnumerator TakeOffSequence()
    {
        isFlying = true;

        // 1. 关门
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(closeTriggerName);
        }

        yield return new WaitForSeconds(doorCloseDelay);

        // 2. 飞船斜上方飞走
        Vector2 dir = flyDirection.normalized;
        float timer = 0f;

        while (timer < flyDuration)
        {
            timer += Time.deltaTime;

            // 加速效果
            float speedFactor = Mathf.Lerp(0.5f, 1.2f, timer / flyDuration);

            shipRoot.position += (Vector3)(dir * flySpeed * speedFactor * Time.deltaTime);
            yield return null;
        }
    }
}
