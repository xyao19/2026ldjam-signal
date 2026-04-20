using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SignalLevel
{
    // 等级名称
    public string levelName;       
    // 触发该等级的最小距离
    public float minDistance;      
    // 蜂鸣器间隔时间
    public float beepInterval; 
    // 是否静音
    public bool mute;         
    // UI 显示颜色(大概率用不到防止意外先填了)
    public Color displayColor; 
    //UI 控制闪烁颜色
    public Color flashColor;
}

[CreateAssetMenu(fileName = "NewSignalData", menuName = "GameData/SignalData Configuration")]
public class SinaglLevelData : ScriptableObject
{
    [Tooltip("排列等级")]
    public List<SignalLevel> signalLevels;

    
    public SignalLevel GetLevelByDistance(float distance)
    {
        foreach (var level in signalLevels)
        {
            // 最近的一个满足条件的等级
            if (distance >= level.minDistance)
            {
                return level;
            }
        }
        // 默认返回最后一项（通常是“无信号”）
        return signalLevels[signalLevels.Count - 1]; 
    }
}