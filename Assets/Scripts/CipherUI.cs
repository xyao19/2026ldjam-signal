using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CipherUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject cipherPanel;              // 整个密码本面板
    public Transform symbolGrid;                // 符号按钮的父物体
    public GameObject symbolButtonPrefab;       // 符号按钮预制体
    public Transform currentCombinationPanel;   // 存放已选符号图标的位置
    public GameObject selectedSymbolPrefab;     // 已选符号的图标预制体（小图片）
    public Button submitButton;
    public TextMeshProUGUI resultText;

    [Header("Event Channels (与B模块对接)")]
    public StringEvent onRequestTranslation;    // 发送请求给 B（组合字符串）
    public StringEvent onTranslationResult;     // 接收 B 返回的翻译结果

    // 内部数据
    private List<CipherSymbol> unlockedSymbols = new List<CipherSymbol>();
    private List<int> currentSelection = new List<int>();   // 存储已选符号的索引
    private List<GameObject> currentSymbolIcons = new List<GameObject>();

    // 符号数据结构（可由B提供，这里先定义）
    [System.Serializable]
    public class CipherSymbol
    {
        public int id;
        public Sprite symbolImage;
        public string symbolChar;    // 用于调试或文本显示
    }

    private void Start()
    {
        // 初始隐藏面板
        cipherPanel.SetActive(false);
        submitButton.onClick.AddListener(OnSubmit);
        // 监听翻译结果
        onTranslationResult.Register(OnTranslationReceived);
    }

    private void OnDestroy()
    {
        onTranslationResult.Unregister(OnTranslationReceived);
    }

    private void Update()
    {
        // 按 Tab 键开关密码本
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCipherPanel();
        }
    }

    private void ToggleCipherPanel()
    {
        bool isActive = !cipherPanel.activeSelf;
        cipherPanel.SetActive(isActive);
        if (isActive)
        {
            RefreshSymbolButtons();      // 每次打开时刷新符号（如果B解锁了新符号）
            RefreshCurrentSelection();   // 刷新已选符号显示
            resultText.text = "";
        }
    }

    // 由 B 调用（通过事件或直接方法），更新已解锁的符号列表
    public void UpdateUnlockedSymbols(List<CipherSymbol> newSymbols)
    {
        unlockedSymbols = newSymbols;
        if (cipherPanel.activeSelf)
            RefreshSymbolButtons();
    }

    private void RefreshSymbolButtons()
    {
        // 清除旧的按钮
        foreach (Transform child in symbolGrid)
            Destroy(child.gameObject);

        // 为每个符号生成按钮
        for (int i = 0; i < unlockedSymbols.Count; i++)
        {
            int index = i; // 闭包捕获
            GameObject btnObj = Instantiate(symbolButtonPrefab, symbolGrid);
            Button btn = btnObj.GetComponent<Button>();
            Image img = btnObj.GetComponentInChildren<Image>();
            if (img != null && unlockedSymbols[i].symbolImage != null)
                img.sprite = unlockedSymbols[i].symbolImage;

            btn.onClick.AddListener(() => AddSymbolToSelection(index));
        }
    }

    private void AddSymbolToSelection(int symbolIndex)
    {
        currentSelection.Add(symbolIndex);
        RefreshCurrentSelection();
    }

    private void RefreshCurrentSelection()
    {
        // 清除旧的图标
        foreach (var icon in currentSymbolIcons)
            Destroy(icon);
        currentSymbolIcons.Clear();

        // 根据 currentSelection 生成小图标
        foreach (int idx in currentSelection)
        {
            GameObject icon = Instantiate(selectedSymbolPrefab, currentCombinationPanel);
            Image img = icon.GetComponent<Image>();
            if (img != null && unlockedSymbols[idx].symbolImage != null)
                img.sprite = unlockedSymbols[idx].symbolImage;
            currentSymbolIcons.Add(icon);
        }
    }

    private void OnSubmit()
    {
        if (currentSelection.Count == 0)
        {
            resultText.text = "请先选择符号！";
            return;
        }
        // 将当前选择的符号组合转为字符串（例如用逗号分隔ID）
        string combination = string.Join(",", currentSelection);
        // 通过事件发送给 B 模块
        onRequestTranslation.Raise(combination);
        // 清空当前选择（可选）
        // currentSelection.Clear();
        // RefreshCurrentSelection();
    }

    private void OnTranslationReceived(string translation)
    {
        resultText.text = translation;
        // 可选：播放音效（调用 C 模块的事件）
    }

    // 供外部调用的清空选择方法
    public void ClearSelection()
    {
        currentSelection.Clear();
        RefreshCurrentSelection();
    }
}