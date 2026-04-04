using UnityEngine;

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
        // AI controlled; don't process player input
        if (isAIControlled)
        {
            return;
        }
        
        // Only respond to input when it's the cat's turn and game is playing
        if (GameManager.Instance.IsMouseTurn || GameManager.Instance.GetGameState() != GameState.Playing)
        {
            return;
        }
        
        HandleInput();
    }
    
    // Handle keyboard input for the cat player
    private void HandleInput()
    {
        // Arrow key input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMoveInDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMoveInDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMoveInDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMoveInDirection(Vector2.right);
        }
        
        // Mouse click input
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClickInput();
        }
        
        // Touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleTouchInput(touch.position);
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
    
    // Set whether this cat is AI controlled
    public void SetAIControlled(bool aiControlled)
    {
        isAIControlled = aiControlled;
    }
}
