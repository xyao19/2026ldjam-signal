using UnityEngine;
using UnityEngine.UI;


public class KeypadButton : MonoBehaviour 
{
    private SignalInputSystem controller;
    private string myValue;

    void Start() 
    {
        //控制器
        controller = GetComponentInParent<SignalInputSystem>();
        
        // 报错提醒
        if (controller == null) {
            Debug.LogError($"在 {gameObject.name} 的父层级中找不到 SignalInputSystem 脚本！");
            return;
        }

        // 自动获取文字
        var textComponent = GetComponentInChildren<Text>();
        if (textComponent != null) 
        {
            myValue = textComponent.text;
            
            //绑定事件
            GetComponent<Button>().onClick.AddListener(() => {if (controller != null) controller.InputCharacter(myValue);});
        } 
        else 
        {
            Debug.LogWarning($"{gameObject.name} 下面找不到 Text 组件！");
        }
    }
}