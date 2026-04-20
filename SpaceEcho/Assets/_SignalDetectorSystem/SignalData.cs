using UnityEngine;

public class SignalData : MonoBehaviour
{
    // 定义物品类型的枚举
    public enum ItemType { Common, KeyPlot,Signaler,Collector }

    [Header("基础信息")]
    public string objectName = "Unknown Object";
    public ItemType objectType = ItemType.Common; 

    public bool isBeingScanned = true;


    //判断是否为重要物品
    public bool IsImportant()    
    {
        return objectType != ItemType.Common;
    }

    //预制体定位使用
    public float uiYOffset = 0.15f;
    


    public Color GetSpecialColor()
    {
        switch (objectType)
        {
            case ItemType.KeyPlot: return Color.cyan;
            case ItemType.Signaler: return new Color(0.6f, 0.2f, 1f); // 紫色
            case ItemType.Collector: return new Color(1f, 0.5f, 0f); // 橙色
            default: return Color.white;
        }
    }

}