using UnityEngine;
using UnityEngine.UI;

public class GameSceneInit : MonoBehaviour
{
    public GameObject pausePanel;
    public Button resumeButton;
    public Button restartButton;
    public Button menuButton;

    private void Start()
    {
        if (GameManager_new.Instance == null) return;

        GameManager_new.Instance.pausePanel = pausePanel;

        resumeButton.onClick.AddListener(GameManager_new.Instance.TogglePause);
        restartButton.onClick.AddListener(GameManager_new.Instance.RestartGame);
        menuButton.onClick.AddListener(GameManager_new.Instance.GoToMenu);
    }
}