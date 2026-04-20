using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BlipRenderer : MonoBehaviour
{
    public RectTransform rectTransform;
    private Image image;
    private Material blipMaterial;

    [Header("闪烁设置")]
    public float baseBlinkSpeed = 2.0f;     // 每秒闪烁次数
    public float outOfRangeSpeed = 0.5f;    // 超出范围时闪烁变慢

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        
        // 实例化材质，确保每个点可以独立控制闪烁相位
        if (image.material != null)
        {
            blipMaterial = new Material(image.material);
            image.material = blipMaterial;
        }
    }

    public void SetBlipState(float intensity, bool isOutOfRange)
    {
        if (blipMaterial == null) return;


        // 设置亮度 (作为 Shader 的 Alpha 倍数)
        blipMaterial.SetFloat("_Intensity", intensity);

        // 设置闪烁速度
        float speed = isOutOfRange ? outOfRangeSpeed : baseBlinkSpeed;
        blipMaterial.SetFloat("_BlinkSpeed", speed);

        // 设置颜色 (可选：超出范围变红)
        Color targetColor = isOutOfRange ? Color.red : Color.green;
        blipMaterial.SetColor("_BlipColor", targetColor);
    }
}