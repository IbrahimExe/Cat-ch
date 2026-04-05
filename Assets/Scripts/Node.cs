using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public string nodeId;
    public List<Node> connectedNodes = new List<Node>();

    private CircleCollider2D circleCollider;

    private void OnEnable()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
        {
            circleCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        circleCollider.isTrigger = true;
    }

    public void AddConnection(Node node)
    {
        if (node != null && !connectedNodes.Contains(node))
        {
            connectedNodes.Add(node);
        }
    }

    public bool IsConnectedTo(Node node)
    {
        return connectedNodes.Contains(node);
    }

    public List<Node> GetConnectedNodes()
    {
        return new List<Node>(connectedNodes);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}