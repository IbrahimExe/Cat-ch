using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI turnIndicatorText;
    [SerializeField]
    private Image timerFillImage;
    
    private void Update()
    {
        if (GameManager.Instance == null)
            return;
        
        UpdateScore();
        UpdateTimer();
        UpdateTurnIndicator();
    }
    
    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Moves: " + GameManager.Instance.MoveCount;
        }
    }
    
    private void UpdateTimer()
    {
        if (timerText != null)
        {
            float timeRemaining = GameManager.Instance.TimerProgress;
            timerText.text = Mathf.Max(0, timeRemaining).ToString("F1");
        }
        
        if (timerFillImage != null)
        {
            timerFillImage.fillAmount = GameManager.Instance.TimerProgress;
        }
    }
    
    private void UpdateTurnIndicator()
    {
        if (turnIndicatorText != null)
        {
            string turnText = GameManager.Instance.IsMouseTurn ? "Mouse's Turn" : "Cat's Turn";
            turnIndicatorText.text = turnText;
        }
    }
}
