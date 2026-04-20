using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 未测试！！！可以利用Slider组件实现信号器强弱，该效果可以整合到SignalUIManager，ObjectDetector当中。
/// 不要轻易使用。
/// </summary>
public class SignalStrenthSlider : MonoBehaviour
{
    public static SignalStrenthSlider Instance;

    [Header("强度可视化 (Strength Display)")]
    [Tooltip("例如：进度条、波形图、或基础信号图标")]
    public Image strengthIndicator;
    [Tooltip("是否使用进度条 FillAmount 表达强度?")]
    public bool useFillAmount = true;

    [Header("特殊闪烁 (Special Flash)")]
    [Tooltip("例如：雷达图标、警示灯、或独立的小红点")]
    public Image specialNotifier; 
    public float flashSpeed = 12f;

    private SignalLevel _currentLevel;
    private bool _isSpecial;

    private void Awake() => Instance = this;

    // 接收来自 Detector 的数据
    public void UpdateUIData(SignalLevel level, bool isSpecial)
    {
        _currentLevel = level;
        _isSpecial = isSpecial;

        // 立即更新强度显示 (基础逻辑，通常无需闪烁)
        UpdateStrengthDisplay();
    }

    private void UpdateStrengthDisplay()
    {
        if (strengthIndicator == null) return;

        // 基础颜色更新 (平滑过渡)
        strengthIndicator.color = Color.Lerp(strengthIndicator.color, _currentLevel.displayColor, Time.deltaTime * 5f);

        // 如果是进度条模式，更新 FillAmount (假设 1.0f 是最远，0.0f 是最近)
        if (useFillAmount)
        {
            // 你需要一个最大检测距离来计算比例，这里假设配置里最远距离是 maxRange
            // 简单处理：根据等级索引设置 Fill (0-1)
            // 假设 0级最弱，Count-1级最强
            // (注意：这里的具体实现取决于你 SignalLevel 列表的排列方式)
            // float fillValue = (float)_currentLevel.levelIndex / (SignalData.Instance.TotalLevels - 1);
            // strengthIndicator.fillAmount = fillValue;
        }
    }

    private void Update()
    {
        // 特殊闪烁逻辑
        HandleSpecialFlashing();
    }

    private void HandleSpecialFlashing()
    {
        if (specialNotifier == null) return;

        if (_isSpecial)
        {
            // 确保通知器是可见的
            if (!specialNotifier.enabled) specialNotifier.enabled = true;

            // 像素风步进闪烁 (更复古，不使用平滑 Lerp)
            float step = Mathf.Floor(Time.time * flashSpeed) % 2;

            //直接开关 Alpha，使用闪烁色
            Color finalColor = _currentLevel.flashColor;
            finalColor.a = (step == 0) ? 1.0f : 0.0f; 
            specialNotifier.color = finalColor;
        }
        else
        {
            // 非特殊道具时，关闭闪烁通知器 (或者让其显示为淡灰色)
            if (specialNotifier.enabled) specialNotifier.enabled = false;
        }
    }

    public void ResetUI()
    {
        _isSpecial = false;
        // 渐变回透明或默认底色
        strengthIndicator.color = Color.gray; 
        if (useFillAmount) strengthIndicator.fillAmount = 0;
        specialNotifier.enabled = false;
    }
}