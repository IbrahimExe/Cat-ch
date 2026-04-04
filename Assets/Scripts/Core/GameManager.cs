using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

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

    // Game settings
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Initialize managers
        if (NodeManager.Instance == null)
        {
            Debug.LogError("NodeManager not found in scene!");
        }

        if (InputManager.Instance == null)
        {
            Debug.LogError("InputManager not found in scene!");
        }
    }

    private void Update()
    {
        if (currentGameState == GameState.Playing)
        {
            UpdateTimer();
        }
    }


    // Initialize a new game with specified settings
    public void StartGame(bool mouseVsCat = false, bool useAI = false, bool hardAI = false)
    {
        gameOver = false;
        moveCount = 0;
        score = 0;
        isMouseTurn = true;
        currentTimerDuration = baseTimerDuration;
        timerCountdown = currentTimerDuration;

        // Initialize players
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


    // Called when a player ends their turn
    public void EndTurn()
    {
        if (currentGameState != GameState.Playing || gameOver)
        {
            return;
        }

        // Check if cat caught the mouse
        if (mouseController.CurrentNode == catController.CurrentNode)
        {
            GameOver();
            return;
        }

        if (isMouseTurn)
        {
            // Mouse just moved, increment score
            score++;
            moveCount++;

            // Reduce timer if threshold is reached
            if (moveCount % movesBeforeTimerReduction == 0)
            {
                currentTimerDuration = Mathf.Max(currentTimerDuration - timerReductionAmount, minTimerDuration);
            }
        }

        // Switch turns
        isMouseTurn = !isMouseTurn;
        timerCountdown = currentTimerDuration;

        // If cat's turn and it's AI controlled, trigger AI move after a small delay
        if (!isMouseTurn && catController.IsAIControlled)
        {
            StartCoroutine(DelayedAIMove());
        }
    }


    // Update the turn timer
    private void UpdateTimer()
    {
        if (gameOver)
        {
            return;
        }

        timerCountdown -= Time.deltaTime;

        if (timerCountdown <= 0)
        {
            // Timer ran out; current player loses
            GameOver();
        }
    }


    // End the game (mouse caught or timer ran out)
    private void GameOver()
    {
        gameOver = true;
        currentGameState = GameState.GameOver;
    }


    // Delay before AI makes its move
    private IEnumerator DelayedAIMove()
    {
        yield return new WaitForSeconds(0.5f);

        // aiController.MakeMove();
    }


    // Get the current game state
    public GameState GetGameState()
    {
        return currentGameState;
    }

    // Set the game state
    public void SetGameState(GameState state)
    {
        currentGameState = state;
    }

    // Check if game is over
    public bool IsGameOver()
    {
        return gameOver;
    }

    // Get the mouse controller
    public MouseController GetMouseController()
    {
        return mouseController;
    }

    // Get the cat controller
    public CatController GetCatController()
    {
        return catController;
    }
}