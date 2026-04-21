using UnityEngine;

public class ParallaxFollow : MonoBehaviour
{
    [Header("跟随目标")]
    [SerializeField] private Transform target;

    [Header("移动系数")]
    [SerializeField] private float moveFactorX = 0.3f;
    [SerializeField] private float moveFactorY = 0f;

    private Vector3 lastTargetPosition;

    private void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 delta = target.position - lastTargetPosition;

        // 形成视差
        transform.position += new Vector3(
            delta.x * moveFactorX,
            delta.y * moveFactorY,
            0f
        );

        lastTargetPosition = target.position;
    }
}
