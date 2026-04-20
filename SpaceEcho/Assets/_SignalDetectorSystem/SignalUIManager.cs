using UnityEngine;
using TMPro;

public class SignalerUIManager : MonoBehaviour
{
    public static SignalerUIManager Instance;
    public GameObject infoPrefab; 

    void Awake()
    {
        Instance = this;
    }

    // INFO UI 显示接口
    public void SignalShowInfo(Transform target, SignalData info, float duration)
    {
        if (infoPrefab == null || info == null) return;

        //控制显示位置，uiUOffset定义在InteractableInfo中
        Vector3 spawnPos = target.position + Vector3.up * info.uiYOffset;

        // 实例化，预制体，位置信息，旋转信息
        GameObject go = Instantiate(infoPrefab, spawnPos, Quaternion.identity);
    
        // 查找背景
        Transform bgTransform = go.transform.Find("Background");
        if (bgTransform != null)
        {
            //查找名称和类型文本组件
            var nameTxt = bgTransform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            var typeTxt = bgTransform.Find("TypeText")?.GetComponent<TextMeshProUGUI>(); 

            //名称文本
            if (nameTxt != null) 
            {
                nameTxt.text = info.objectName; 
            }
            //类型文本
            if (typeTxt != null) 
            {
                typeTxt.text = $"[{info.objectType}]";
            }
        }
        else
            {Debug.LogWarning("未能在预制体中找到 Background 节点！");}
        Destroy(go, duration);
    }    
}