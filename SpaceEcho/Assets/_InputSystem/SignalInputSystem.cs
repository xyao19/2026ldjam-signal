using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SignalInputSystem : MonoBehaviour 
{
    [Header("显示区域")]
    public Text displayField; 
    public int maxCharacters = 10;

    private StringBuilder currentInput = new StringBuilder();
    void Start()
    {
        RefreshUI() ;
    }

    
    //输入文本的函数调用
    public void InputCharacter(string value) 
    {
        if (currentInput.Length < maxCharacters) 
        {
            currentInput.Append(value);
            Debug.Log("输入: " + value);
            RefreshUI();
        }
    }


    //删除功能
    public void DeleteLast() 
    {
        if (currentInput.Length > 0) 
        {
            currentInput.Remove(currentInput.Length - 1, 1);
            RefreshUI();
        }
    }

    // 确认功能
    public void ConfirmInput() 
    {
        string finalCode = currentInput.ToString();
        // TODO : 这里应该有一次与密码本的验证，需要补充
        //SignalManager.Instance.ProcessSignal(finalCode); 
    }



    // TODO:应该增加些便利性功能，来不及了先放着

    //刷新文本显示
    private void RefreshUI() 
    {
        displayField.text = currentInput.ToString();
    }
}