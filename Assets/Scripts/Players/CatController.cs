using UnityEngine;
using UnityEngine.InputSystem;

public class CatController : PlayerBase
{
    [SerializeField]
    private bool isAIControlled = false;

    private InputManager inputManager;
    private float inputBufferDuration = 0.15f;
    private float inputBufferTimer = 0f;
    private bool isBufferingInput = false;

    public bool IsAIControlled => isAIControlled;

    protected override void Awake()
    {
        base.Awake();
        inputManager = InputManager.Instance;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void Update()
    {
        if (isAIControlled)
        {
            return;
        }

        if (GameManager.Instance.IsMouseTurn || GameManager.Instance.GetGameState() != GameState.Playing)
        {
            return;
        }

        HandleInput();
    }

    private void HandleInput()
    {
        // Stop all input handling if game is not playing
        if (GameManager.Instance.GetGameState() != GameState.Playing)
        {
            ResetInputBuffer();
            return;
        }

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        // Start input buffer if any arrow key is pressed
        if (!isBufferingInput && (keyboard.upArrowKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame ||
            keyboard.leftArrowKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame))
        {
            isBufferingInput = true;
            inputBufferTimer = 0f;
        }

        // Update input buffer timer
        if (isBufferingInput)
        {
            inputBufferTimer += Time.deltaTime;

            // After buffer window closes, make the move
            if (inputBufferTimer >= inputBufferDuration)
            {
                ExecuteMovement(keyboard);
                isBufferingInput = false;
            }
        }

        // Handle mouse click (bypasses buffer)
        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            HandleMouseClickInput();
        }

        // Handle touch input
        Touchscreen touchscreen = Touchscreen.current;
        if (touchscreen != null && touchscreen.touches.Count > 0)
        {
            var touch = touchscreen.touches[0];
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                HandleTouchInput(touch.position.ReadValue());
            }
        }
    }

    private void ExecuteMovement(Keyboard keyboard)
    {
        Vector2 direction = Vector2.zero;

        // Collect all currently held keys
        if (keyboard.upArrowKey.isPressed)
            direction += Vector2.up;
        if (keyboard.downArrowKey.isPressed)
            direction += Vector2.down;
        if (keyboard.leftArrowKey.isPressed)
            direction += Vector2.left;
        if (keyboard.rightArrowKey.isPressed)
            direction += Vector2.right;

        // Only move if there's a direction input
        if (direction != Vector2.zero)
        {
            TryMoveInDirection(direction.normalized);
        }
    }

    public void ResetInputBuffer()
    {
        isBufferingInput = false;
        inputBufferTimer = 0f;
    }

    private void TryMoveInDirection(Vector2 direction)
    {
        Node targetNode = FindBestNodeInDirection(direction);
        if (targetNode != null && currentNode.IsConnectedTo(targetNode))
        {
            StartCoroutine(MoveAndEndTurn(targetNode));
        }
    }

    private System.Collections.IEnumerator MoveAndEndTurn(Node targetNode)
    {
        MoveToNode(targetNode);

        // Wait for movement to complete
        while (isMoving)
        {
            yield return null;
        }

        // Now end turn after movement is done
        GameManager.Instance.EndTurn();
    }

    private Node FindBestNodeInDirection(Vector2 direction)
    {
        Node bestNode = null;
        float bestScore = -2f;

        foreach (Node connectedNode in currentNode.GetConnectedNodes())
        {
            Vector2 directionToNode = (connectedNode.GetPosition() - currentNode.GetPosition()).normalized;
            float dotProduct = Vector2.Dot(directionToNode, direction);

            if (dotProduct > bestScore)
            {
                bestScore = dotProduct;
                bestNode = connectedNode;
            }
        }

        return bestNode;
    }

    private void HandleMouseClickInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            Node clickedNode = hit.collider.GetComponent<Node>();
            if (clickedNode != null && currentNode.IsConnectedTo(clickedNode))
            {
                StartCoroutine(MoveAndEndTurn(clickedNode));
            }
        }
    }

    private void HandleTouchInput(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0));
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            Node touchedNode = hit.collider.GetComponent<Node>();
            if (touchedNode != null && currentNode.IsConnectedTo(touchedNode))
            {
                StartCoroutine(MoveAndEndTurn(touchedNode));
            }
        }
    }

    public void SetAIControlled(bool aiControlled)
    {
        isAIControlled = aiControlled;
    }
}