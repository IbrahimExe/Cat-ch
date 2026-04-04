using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public abstract class PlayerBase : MonoBehaviour
{
    [SerializeField]
    protected Node currentNode;

    protected SpriteRenderer spriteRenderer;
    protected float moveSpeed = 3f;
    protected bool isMoving = false;

    public Node CurrentNode => currentNode;
    public bool IsMoving => isMoving;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }


    // Initialize the player at a starting node
    public virtual void Initialize(Node startingNode)
    {
        if (startingNode == null)
        {
            Debug.LogError($"{gameObject.name} initialized with null node!");
            return;
        }

        currentNode = startingNode;
        transform.position = currentNode.GetPosition();
    }

    // Attempt to move the player to a target node
    public virtual void MoveToNode(Node targetNode)
    {
        if (targetNode == null || isMoving)
        {
            return;
        }

        if (!currentNode.IsConnectedTo(targetNode))
        {
            Debug.LogWarning($"Cannot move from {currentNode.nodeId} to {targetNode.nodeId} - not connected!");
            return;
        }

        StartCoroutine(SmoothMoveTo(targetNode));
    }

    // Smooth animation from current node to target node
    protected virtual IEnumerator SmoothMoveTo(Node targetNode)
    {
        isMoving = true;
        Vector3 startPosition = currentNode.GetPosition();
        Vector3 endPosition = targetNode.GetPosition();
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            yield return null;
        }

        transform.position = endPosition;
        currentNode = targetNode;
        isMoving = false;
    }


    // Get the current node the player is on
    public Node GetCurrentNode()
    {
        return currentNode;
    }

    // Teleport player instantly to a node maybe for a respawn idk yet
    public void TeleportToNode(Node targetNode)
    {
        if (targetNode == null) return;

        StopAllCoroutines();
        currentNode = targetNode;
        transform.position = currentNode.GetPosition();
        isMoving = false;
    }
}