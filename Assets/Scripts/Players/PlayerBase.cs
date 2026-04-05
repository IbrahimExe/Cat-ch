using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour
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

    public Node GetCurrentNode()
    {
        return currentNode;
    }

    public void TeleportToNode(Node targetNode)
    {
        if (targetNode == null) return;

        StopAllCoroutines();
        currentNode = targetNode;
        transform.position = currentNode.GetPosition();
        isMoving = false;
    }
}