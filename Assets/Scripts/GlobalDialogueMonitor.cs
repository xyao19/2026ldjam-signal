using UnityEngine;
using NodeCanvas.DialogueTrees;
using System.Collections.Generic;

/// <summary>
/// 全局对话树监听器。
/// 当场景中任意一个对话树启动时，禁用指定组件；当所有对话树都结束后，恢复组件。
/// 挂载在任何 GameObject 上即可，无需额外配置。
/// </summary>
public class GlobalDialogueMonitor : MonoBehaviour
{
    [Tooltip("对话树启动时需要禁用的组件（如 Renderer、Collider、Behaviour 等）")]
    public Behaviour targetComponent;

    [Tooltip("是否同时禁用/恢复子物体上所有同类型的组件")]
    public bool affectChildren = false;

    [Tooltip("扫描对话树状态的间隔（秒）")]
    public float checkInterval = 0.3f;

    // 记录当前所有对话树的运行状态
    private Dictionary<DialogueTreeController, bool> controllerStates = new Dictionary<DialogueTreeController, bool>();
    private int activeDialogueCount = 0;           // 当前正在运行的对话树数量
    private float timer = 0f;

    // 记录受影响组件的原始启用状态（仅 affectChildren = true 时使用）
    private Dictionary<Behaviour, bool> originalStates = new Dictionary<Behaviour, bool>();

    void OnEnable()
    {
        if (targetComponent == null)
        {
            Debug.LogError($"[GlobalDialogueMonitor] 物体 '{name}' 未指定 targetComponent，脚本已禁用。", this);
            enabled = false;
            return;
        }

        // 初始扫描一次
        ScanAndUpdate();
        timer = checkInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            ScanAndUpdate();
        }
    }

    void OnDisable()
    {
        // 确保组件恢复启用
        RestoreComponents();
        controllerStates.Clear();
    }

    private void ScanAndUpdate()
    {
        // 1. 找到场景中所有激活的 DialogueTreeController
        var controllers = FindObjectsOfType<DialogueTreeController>(true);

        // 2. 更新字典：移除已被销毁的 controller
        var toRemove = new List<DialogueTreeController>();
        foreach (var c in controllerStates.Keys)
        {
            if (c == null)
                toRemove.Add(c);
        }
        foreach (var c in toRemove)
            controllerStates.Remove(c);

        // 3. 记录新的活跃计数（当前所有正在运行的对话树数量）
        int newActiveCount = 0;
        foreach (var controller in controllers)
        {
            bool isRunning = controller.isRunning;   // 关键属性：对话树是否正在运行
            if (!controllerStates.ContainsKey(controller))
            {
                controllerStates[controller] = isRunning;
            }
            else
            {
                // 检测状态变化（可选，用于调试）
                bool previous = controllerStates[controller];
                if (previous != isRunning)
                {
                    controllerStates[controller] = isRunning;
                    // 不需要单独处理，因为最终 newActiveCount 会汇总所有运行中的 controller
                }
            }
            if (isRunning)
                newActiveCount++;
        }

        // 4. 处理活跃计数的变化
        if (newActiveCount > 0 && activeDialogueCount == 0)
        {
            // 从无对话到有对话：禁用组件
            DisableComponents();
        }
        else if (newActiveCount == 0 && activeDialogueCount > 0)
        {
            // 从有对话到无对话：恢复组件
            RestoreComponents();
        }

        activeDialogueCount = newActiveCount;
    }

    private void DisableComponents()
    {
        if (affectChildren)
        {
            originalStates.Clear();
            var allComponents = GetComponentsInChildren<Behaviour>();
            foreach (var comp in allComponents)
            {
                if (comp != null && comp.GetType() == targetComponent.GetType())
                {
                    originalStates[comp] = comp.enabled;
                    comp.enabled = false;
                }
            }
        }
        else
        {
            targetComponent.enabled = false;
        }
        Debug.Log($"[GlobalDialogueMonitor] 检测到对话树启动，已禁用组件：{targetComponent.name}");
    }

    private void RestoreComponents()
    {
        if (affectChildren)
        {
            foreach (var kv in originalStates)
            {
                if (kv.Key != null)
                    kv.Key.enabled = kv.Value;
            }
            originalStates.Clear();
        }
        else
        {
            if (targetComponent != null)
                targetComponent.enabled = true;
        }
        Debug.Log($"[GlobalDialogueMonitor] 所有对话树已结束，已恢复组件：{targetComponent.name}");
    }
}