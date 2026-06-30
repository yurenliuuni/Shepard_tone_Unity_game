using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager_new : MonoBehaviour
{
    public static GameManager_new Instance { get; private set; }

    [Header("Game Data")]
    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;
    public int coins { get; private set; } = 0;

    [Header("UI")]
    public GameObject pausePanel;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        // 只在 Menu 場景初始化，不要自動 NewGame
        // NewGame 改成由 StartButton 呼叫
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    // ── Pause ──────────────────────────────
    public void TogglePause()
    {
        // Menu 場景不需要 pause
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        isPaused = !isPaused;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    // ── Game Flow ───────────────────────────
    public void NewGame()
    {
        lives = 3;
        coins = 0;
        Time.timeScale = 1f;
        LoadLevel(1, 1);
    }

    public void GameOver()
    {
        NewGame();
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;
        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
            LoadLevel(world, stage);
        else
            GameOver();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        lives = 3;
        coins = 0;
        LoadLevel(world, stage);
    }

    public void GoToMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        SceneManager.LoadScene(0);
    }

    // ── Coins & Lives ───────────────────────
    public void AddCoin()
    {
        coins++;
        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
    }
} 