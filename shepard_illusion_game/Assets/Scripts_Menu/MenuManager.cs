using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject theoryPanel;
    public GameObject operationPanel;
    public GameObject menuPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenTheory()
    {
        theoryPanel.SetActive(true);
    }
    public void OpenOp()
    {
        operationPanel.SetActive(true);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
    }


    public void CloseTheory()
    {
        theoryPanel.SetActive(false);
    }
    public void CloseOp()
    {
        operationPanel.SetActive(false);
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
