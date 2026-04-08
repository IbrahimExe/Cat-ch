using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI finalScoreText;
    [SerializeField]
    private TextMeshProUGUI winnerText;
    
    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            UpdateScoreDisplay();
        }
    }
    
    private void UpdateScoreDisplay()
    {
        int finalScore = GameManager.Instance.MoveCount;
        
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore + " moves survived";
        }
        
        if (winnerText != null)
        {
            bool mouseWon = GameManager.Instance.IsMouseTurn;
            winnerText.text = mouseWon ? "Mouse Escaped!" : "Cat Caught the Mouse!";
        }
    }
    
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
