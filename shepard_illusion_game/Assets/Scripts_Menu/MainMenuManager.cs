using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject mainPanel;       // 主選單面板
    public GameObject howToPlayPanel;  // 說明面板

    private void Start()
    {
        ShowMain();
    }

    // 開始遊戲
    public void OnStartGame()
    {
        SceneManager.LoadScene(1); // Scene 1 = 遊戲場景
    }

    // 顯示說明
    public void OnHowToPlay()
    {
        mainPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    // 返回主選單
    public void OnBack()
    {
        ShowMain();
    }

    // 退出遊戲
    public void OnQuit()
    {
        Application.Quit();
        // Editor 裡測試用：
        // UnityEditor.EditorApplication.isPlaying = false;
    }

    private void ShowMain()
    {
        mainPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }
}