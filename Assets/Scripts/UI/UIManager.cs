using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private GameObject gameplayHUDPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject settingsPanel;
    [SerializeField]
    private GameObject aboutPanel;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    private void Start()
    {
        ShowMainMenu();
    }
    
    public void ShowMainMenu()
    {
        SetAllPanelsInactive();
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        Time.timeScale = 1f;
    }
    
    public void ShowGameplay()
    {
        SetAllPanelsInactive();
        if (gameplayHUDPanel != null)
            gameplayHUDPanel.SetActive(true);
        Time.timeScale = 1f;
    }
    
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void ShowSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }
    
    public void HideSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    
    public void ShowAbout()
    {
        if (aboutPanel != null)
            aboutPanel.SetActive(true);
    }
    
    public void HideAbout()
    {
        if (aboutPanel != null)
            aboutPanel.SetActive(false);
    }
    
    private void SetAllPanelsInactive()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (gameplayHUDPanel != null)
            gameplayHUDPanel.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        if (aboutPanel != null)
            aboutPanel.SetActive(false);
    }
}
