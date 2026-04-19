using System.Collections.Generic;
using UnityEngine;

public class MockCipherManager : MonoBehaviour
{
    public CipherUI cipherUI;
    public StringEvent mockTranslationResult;

    private void Start()
    {
        // 模拟一些符号数据
        var mockSymbols = new List<CipherUI.CipherSymbol>();
        for (int i = 0; i < 6; i++)
        {
            mockSymbols.Add(new CipherUI.CipherSymbol
            {
                id = i,
                symbolChar = $"S{i}",
                symbolImage = null // 或加载测试图片
            });
        }
        cipherUI.UpdateUnlockedSymbols(mockSymbols);

        // 监听翻译请求
        cipherUI.onRequestTranslation.Register(OnMockTranslationRequest);
    }

    private void OnMockTranslationRequest(string combination)
    {
        Debug.Log($"收到组合: {combination}");
        // 模拟翻译
        string fakeResult = "模拟翻译：" + combination;
        mockTranslationResult.Raise(fakeResult);
    }
}