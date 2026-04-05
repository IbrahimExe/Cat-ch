using UnityEngine;
using UnityEngine.InputSystem;

public class CatController : PlayerBase
{
    [SerializeField]
    private bool isAIControlled = false;

    private InputManager inputManager;

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
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        // Check for any arrow key pressed this frame
        if (keyboard.upArrowKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame ||
            keyboard.leftArrowKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
        {
            HandleMovementInput(keyboard);
        }

        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            HandleMouseClickInput();
        }

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

    private void HandleMovementInput(Keyboard keyboard)
    {
        Vector2 direction = Vector2.zero;

        // Check currently held keys to build a combined direction
        if (keyboard.upArrowKey.isPressed)
            direction += Vector2.up;
        if (keyboard.downArrowKey.isPressed)
            direction += Vector2.down;
        if (keyboard.leftArrowKey.isPressed)
            direction += Vector2.left;
        if (keyboard.rightArrowKey.isPressed)
            direction += Vector2.right;

        if (direction != Vector2.zero)
        {
            TryMoveInDirection(direction.normalized);
        }
    }

    private void TryMoveInDirection(Vector2 direction)
    {
        Node targetNode = FindBestNodeInDirection(direction);
        if (targetNode != null && currentNode.IsConnectedTo(targetNode))
        {
            MoveToNode(targetNode);
            GameManager.Instance.EndTurn();
        }
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
                MoveToNode(clickedNode);
                GameManager.Instance.EndTurn();
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
                MoveToNode(touchedNode);
                GameManager.Instance.EndTurn();
            }
        }
    }

    public void SetAIControlled(bool aiControlled)
    {
        isAIControlled = aiControlled;
    }
}