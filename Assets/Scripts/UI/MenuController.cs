using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlaySolo()
    {
        StartGame(useAI: true, hardAI: false);
    }
    
    public void PlaySoloHard()
    {
        StartGame(useAI: true, hardAI: true);
    }
    
    public void PlayVsFriend()
    {
        StartGame(useAI: false, hardAI: false);
    }
    
    private void StartGame(bool useAI, bool hardAI)
    {
        SceneManager.LoadScene("Game");
        
        // Wait for scene to load, then start game
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(mouseVsCat: false, useAI: useAI, hardAI: hardAI);
            UIManager.Instance.ShowGameplay();
        }
    }
    
    public void OpenSettings()
    {
        UIManager.Instance.ShowSettings();
    }
    
    public void OpenAbout()
    {
        UIManager.Instance.ShowAbout();
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
