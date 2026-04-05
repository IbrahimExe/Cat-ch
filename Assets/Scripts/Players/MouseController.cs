using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : PlayerBase
{
    private InputManager inputManager;

    protected override void Awake()
    {
        base.Awake();
        inputManager = InputManager.Instance;

        if (spriteRenderer != null)
        {
            //spriteRenderer.color = Color.yellow;
        }
    }

    private void Update()
    {
        // Only respond to input when it's the mouse's turn and game is playing
        if (!GameManager.Instance.IsMouseTurn || GameManager.Instance.GetGameState() != GameState.Playing)
        {
            return;
        }

        HandleInput();
    }


    // Handle keyboard and mouse input for the mouse player
    private void HandleInput()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        // Keyboard input (WASD)
        if (keyboard.wKey.wasPressedThisFrame)
        {
            TryMoveInDirection(Vector2.up);
        }
        else if (keyboard.aKey.wasPressedThisFrame)
        {
            TryMoveInDirection(Vector2.left);
        }
        else if (keyboard.sKey.wasPressedThisFrame)
        {
            TryMoveInDirection(Vector2.down);
        }
        else if (keyboard.dKey.wasPressedThisFrame)
        {
            TryMoveInDirection(Vector2.right);
        }

        // Mouse click input; click on a node to move there
        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            HandleMouseClickInput();
        }

        // Touch input
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

    // Try to move in a direction based on keyboard input
    private void TryMoveInDirection(Vector2 direction)
    {
        Node targetNode = FindNodeInDirection(direction);
        if (targetNode != null && currentNode.IsConnectedTo(targetNode))
        {
            MoveToNode(targetNode);
            GameManager.Instance.EndTurn();
        }
    }


    // Find the closest connected node in a given direction
    private Node FindNodeInDirection(Vector2 direction)
    {
        Node bestNode = null;
        float bestDot = -1f;

        foreach (Node connectedNode in currentNode.GetConnectedNodes())
        {
            Vector2 directionToNode = (connectedNode.GetPosition() - currentNode.GetPosition()).normalized;
            float dot = Vector2.Dot(directionToNode, direction.normalized);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestNode = connectedNode;
            }
        }

        return bestNode;
    }

    // Handle mouse click input on nodes
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

    // Handle touch input on nodes
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
}