using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private MouseController mouseController;
    [SerializeField]
    private CatController catController;
    [SerializeField]
    private Node mouseStartingNode;
    [SerializeField]
    private Node catStartingNode;

    [SerializeField]
    private float baseTimerDuration = 5f;
    [SerializeField]
    private float minTimerDuration = 1f;
    [SerializeField]
    private int movesBeforeTimerReduction = 10;
    [SerializeField]
    private float timerReductionAmount = 0.5f;

    private GameState currentGameState = GameState.Menu;
    private bool isMouseTurn = true;
    private float currentTimerDuration;
    private float timerCountdown;
    private int moveCount = 0;
    private int score = 0;
    private bool gameOver = false;

    public bool IsMouseTurn => isMouseTurn;
    public int Score => score;
    public int MoveCount => moveCount;
    public float TimerProgress => timerCountdown / currentTimerDuration;

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
        if (NodeManager.Instance == null)
        {
            Debug.LogError("NodeManager not found in scene!");
            return;
        }

        if (InputManager.Instance == null)
        {
            Debug.LogError("InputManager not found in scene!");
            return;
        }

        StartGame();
    }

    private void Update()
    {
        if (currentGameState == GameState.Playing)
        {
            UpdateTimer();
        }
    }

    public void StartGame(bool mouseVsCat = false, bool useAI = false, bool hardAI = false)
    {
        gameOver = false;
        moveCount = 0;
        score = 0;
        isMouseTurn = true;
        currentTimerDuration = baseTimerDuration;
        timerCountdown = currentTimerDuration;

        if (mouseStartingNode != null && mouseController != null)
        {
            mouseController.Initialize(mouseStartingNode);
        }

        if (catStartingNode != null && catController != null)
        {
            catController.Initialize(catStartingNode);
            catController.SetAIControlled(useAI);
        }

        currentGameState = GameState.Playing;
    }

    public void EndTurn()
    {
        if (currentGameState != GameState.Playing || gameOver)
        {
            return;
        }

        if (mouseController == null || catController == null)
        {
            Debug.LogError("Player controllers not assigned!");
            return;
        }

        if (mouseController.CurrentNode == catController.CurrentNode)
        {
            GameOver();
            return;
        }

        if (isMouseTurn)
        {
            score++;
            moveCount++;

            if (moveCount % movesBeforeTimerReduction == 0)
            {
                currentTimerDuration = Mathf.Max(currentTimerDuration - timerReductionAmount, minTimerDuration);
            }
        }

        isMouseTurn = !isMouseTurn;
        timerCountdown = currentTimerDuration;

        if (!isMouseTurn && catController.IsAIControlled)
        {
            StartCoroutine(DelayedAIMove());
        }
    }

    private void UpdateTimer()
    {
        if (gameOver || currentGameState != GameState.Playing)
        {
            return;
        }

        timerCountdown -= Time.deltaTime;

        if (timerCountdown <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        currentGameState = GameState.GameOver;

        // Reset input buffers to prevent delayed moves
        if (mouseController != null)
            mouseController.ResetInputBuffer();
        if (catController != null)
            catController.ResetInputBuffer();
    }

    private IEnumerator DelayedAIMove()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }

    public void SetGameState(GameState state)
    {
        currentGameState = state;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public MouseController GetMouseController()
    {
        return mouseController;
    }

    public CatController GetCatController()
    {
        return catController;
    }
}