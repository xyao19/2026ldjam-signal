using UnityEngine;

public class PlanetParallax : MonoBehaviour
{
    [Header("跟随目标")]
    [SerializeField] private Transform target;

    [Header("平移系数")]
    [SerializeField] private float moveFactorX = 0.15f;
    [SerializeField] private float moveFactorY = 0f;

    [Header("旋转设置")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool rotateWithPlayerDirection = true;

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

        // 平行移动
        transform.position += new Vector3(
            delta.x * moveFactorX,
            delta.y * moveFactorY,
            0f
        );

        // 旋转
        if (Mathf.Abs(delta.x) > 0.0001f)
        {
            float direction = Mathf.Sign(delta.x);

            if (rotateWithPlayerDirection)
            {
                // 玩家往右，星球按一个方向转；往左，反向转
                transform.Rotate(0f, 0f, direction * rotationSpeed * Time.deltaTime);
            }
            else
            {
                // 不管玩家方向，始终同方向旋转
                transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
            }
        }

        lastTargetPosition = target.position;
    }
}
