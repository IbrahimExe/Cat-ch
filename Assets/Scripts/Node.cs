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
    
    // Add a connection to another node
    public void AddConnection(Node node)
    {
        if (node != null && !connectedNodes.Contains(node))
        {
            connectedNodes.Add(node);
        }
    }
    
    // Check if this node is directly connected to another
    public bool IsConnectedTo(Node node)
    {
        return connectedNodes.Contains(node);
    }
    
    // Get all nodes connected to this one
    public List<Node> GetConnectedNodes()
    {
        return new List<Node>(connectedNodes);
    }
    
    // Get world position of this node
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}