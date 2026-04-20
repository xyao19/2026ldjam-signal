using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("È«¾ÖUI")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject settingButton;


    public GameState CurrentState { get; private set; } = GameState.Playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;

        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }

        SceneManager.LoadScene("MainScene");
    }

    public void OpenSetting()
    {
        CurrentState = GameState.Paused;
        Time.timeScale = 0f;

        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
        }
    }

    public void CloseSetting()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
    }

    public void QuitToStartScene()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;

        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }

        SceneManager.LoadScene("StartScene");
    }
}