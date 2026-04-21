using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

// 注意分类的写法，这会在NodeCanvas的菜单中显示为"Custom/Teleport"
[Category("Custom/Teleport")]
// 指定这个Task需要一个带有Transform组件的GameObject作为执行者（Agent）
public class TeleportAction : ActionTask<Transform>
{
    // 在Inspector中指定要传送到的位置
    public BBParameter<Vector3> targetPosition;
    // 也可以直接拖拽一个目标GameObject作为传送点
    public BBParameter<GameObject> teleportTarget;

    // 当任务开始执行时会调用这个方法
    protected override void OnExecute()
    {
        // 优先检查是否指定了传送目标物体
        if (teleportTarget.value != null)
        {
            // 将Agent（角色）的位置设置为传送目标物体的位置
            agent.position = teleportTarget.value.transform.position;
        }
        else
        {
            // 否则使用手动输入的坐标
            agent.position = targetPosition.value;
        }

        Debug.Log($"{agent.name} 已传送到 {agent.position}");

        // 任务执行成功，对话树将继续向下执行
        EndAction(true);
    }
}