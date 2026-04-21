using System.Collections;
using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

public class ShipLeave : MonoBehaviour
{
    [UnityEngine.Header("引用")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Transform shipRoot;

    [UnityEngine.Header("门关闭设置")]
    [SerializeField] private string closeTriggerName = "CloseDoor";
    [SerializeField] private float doorCloseDelay = 1f;

    [UnityEngine.Header("飞船起飞设置")]
    [SerializeField] private Vector2 flyDirection = new Vector2(1f, 1f);
    [SerializeField] private float flySpeed = 5f;
    [SerializeField] private float flyDuration = 2f;

    private bool isTakingOff = false;

    /// <summary>
    /// 供外部调用的起飞方法（协程非阻塞）
    /// </summary>
    public void TakeOff()
    {
        if (!isTakingOff)
        {
            StartCoroutine(TakeOffSequence());
        }
    }

    private IEnumerator TakeOffSequence()
    {
        isTakingOff = true;

        // 1. 关门动画
        if (doorAnimator != null)
            doorAnimator.SetTrigger(closeTriggerName);

        yield return new WaitForSeconds(doorCloseDelay);

        // 2. 飞船飞走（加速效果）
        Vector2 dir = flyDirection.normalized;
        float timer = 0f;
        while (timer < flyDuration)
        {
            timer += Time.deltaTime;
            float speedFactor = Mathf.Lerp(0.5f, 1.2f, timer / flyDuration);
            shipRoot.position += (Vector3)(dir * flySpeed * speedFactor * Time.deltaTime);
            yield return null;
        }

        isTakingOff = false;
    }

    /// <summary>
    /// 内部类：NodeCanvas 可调用的 Action Task
    /// </summary>
    [Category("✫ Custom")]
    [Description("让飞船起飞（关门 + 飞走 + 激活物体）")]
    public class ShipLeaveTask : ActionTask<ShipLeave>
    {
        protected override void OnExecute()
        {
            if (agent == null)
            {
                Debug.LogError("ShipLeaveTask 需要 agent 为 ShipLeave 组件");
                EndAction(false);
                return;
            }

            agent.TakeOff();
            EndAction(true);
        }
    }
}