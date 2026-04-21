using UnityEngine;
using UnityEngine.UI;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject;   // 要开关的物体
    public Button targetButton;       // 触发按钮（可选，也可通过其他方式调用）

    void Start()
    {
        if (targetButton != null)
            targetButton.onClick.AddListener(Toggle);
    }

    public void Toggle()
    {
        if (targetObject != null)
            targetObject.SetActive(!targetObject.activeSelf);
    }
}