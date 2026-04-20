using UnityEngine;
using System.Collections.Generic;

public class RadarManager : MonoBehaviour
{
    public static RadarManager Instance;

    [Header("设置")]
    public Transform playerTransform;      // 雷达中心 玩家
    public RectTransform radarPanel;      // 雷达面板 UI
    public GameObject blipPrefab;         // 雷达点的预制体
    public float maxRadarRange = 20f;     // 雷达最大探测范围
    public float radarUIScale = 20f;     // 像素大小

    // 存储当前雷达上的点：<物体, UI点>
    private Dictionary<GameObject, BlipRenderer> activeBlips = new Dictionary<GameObject, BlipRenderer>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // 每一帧更新所有点的位置，保持相对于玩家的正确投影
        if (playerTransform == null || radarPanel == null) return;

        // 创建一个临时列表存储需要移除的键，避免在循环中修改字典
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var kvp in activeBlips)
        {
            if (kvp.Key == null) // 物体被销毁了
            {
                toRemove.Add(kvp.Key);
                continue;
            }

            UpdateBlipPosition(kvp.Key, kvp.Value);
        }

        // 处理移除
        foreach (var obj in toRemove)
        {
            if (activeBlips[obj] != null) Destroy(activeBlips[obj].gameObject);
            activeBlips.Remove(obj);
        }
    }

    
    public void RegisterDetectedObject(GameObject obj)
    {
        if (activeBlips.ContainsKey(obj) || blipPrefab == null) return;

        // 生成 UI 点
        GameObject blipGo = Instantiate(blipPrefab, radarPanel);
        BlipRenderer renderer = blipGo.GetComponent<BlipRenderer>();
        
        if (renderer != null)
        {
            activeBlips.Add(obj, renderer);
            UpdateBlipPosition(obj, renderer); // 立即初始化位置
        }
    }

    private void UpdateBlipPosition(GameObject target, BlipRenderer blip)
    {
        // 计算世界坐标下的相对矢量 (World Vector)
        Vector3 relativePos = target.transform.position - playerTransform.position;
        
        // 转化为极坐标系 (Polar Coordinates)
        // radius = 距离, angle = 角度（弧度）
        float radius = relativePos.magnitude;

        // 如果超出雷达范围，将其“钉”在边缘，或者直接隐藏
        bool isOutOfRange = radius > maxRadarRange;
        float finalRadius = Mathf.Min(radius, maxRadarRange);

        // 使用 Atan2 计算角度 (x, y)，得到相对于玩家前方的角度
        // 注意：这里假设 2D 游戏是 X-Y 平面，如果需要相对于玩家朝向，需要减去玩家的 eulerAngles.y
        float angleRad = Mathf.Atan2(relativePos.y, relativePos.x);

        // 投影成 UI 平面的点 (Project to UI)
        // 将世界距离映射到 UI 像素距离
        float uiRadius = (finalRadius / maxRadarRange) * radarUIScale;

        // 极坐标转直角坐标 (Polar to Cartesian for UI)
        float uiX = Mathf.Cos(angleRad) * uiRadius;
        float uiY = Mathf.Sin(angleRad) * uiRadius;

        // 应用位置
        blip.rectTransform.anchoredPosition = new Vector2(uiX, uiY);
        
        // 根据状态设置效果 (可选：越远越淡，超出范围变红)
        float intensity = isOutOfRange ? 0.3f : (1.0f - (radius / maxRadarRange) * 0.5f);
        blip.SetBlipState(intensity, isOutOfRange);
    }

    public void RemoveObject(GameObject obj)
    {
        if (activeBlips.ContainsKey(obj))
        {
            if (activeBlips[obj] != null) Destroy(activeBlips[obj].gameObject);
            activeBlips.Remove(obj);
        }
    }
}