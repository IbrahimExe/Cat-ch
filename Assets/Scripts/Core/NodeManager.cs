using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    [SerializeField]
    private Node centerNode;
    [SerializeField]
    private Node topMiddleNode;
    [SerializeField]
    private Node topLeftNode;
    [SerializeField]
    private Node topRightNode;
    [SerializeField]
    private Node bottomLeftNode;
    [SerializeField]
    private Node bottomRightNode;

    private Dictionary<string, Node> nodeMap = new Dictionary<string, Node>();

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
        InitializeNodeMap();
        SetupConnections();
    }

    // Create a dictionary mapping of all nodes for quick access
    private void InitializeNodeMap()
    {
        AddNodeToMap(centerNode, "CENTER");
        AddNodeToMap(topMiddleNode, "TOP_MIDDLE");
        AddNodeToMap(topLeftNode, "TOP_LEFT");
        AddNodeToMap(topRightNode, "TOP_RIGHT");
        AddNodeToMap(bottomLeftNode, "BOTTOM_LEFT");
        AddNodeToMap(bottomRightNode, "BOTTOM_RIGHT");
    }

    // Setup all node connections based on map layout
    private void SetupConnections()
    {
        // CENTER connects to: TOP_MIDDLE, BOTTOM_LEFT, BOTTOM_RIGHT
        centerNode.AddConnection(topMiddleNode);
        centerNode.AddConnection(bottomLeftNode);
        centerNode.AddConnection(bottomRightNode);

        // TOP_MIDDLE connects to: CENTER, TOP_LEFT, TOP_RIGHT
        topMiddleNode.AddConnection(centerNode);
        topMiddleNode.AddConnection(topLeftNode);
        topMiddleNode.AddConnection(topRightNode);

        // TOP_LEFT connects to: TOP_MIDDLE, BOTTOM_LEFT
        topLeftNode.AddConnection(topMiddleNode);
        topLeftNode.AddConnection(bottomLeftNode);

        // TOP_RIGHT connects to: TOP_MIDDLE, BOTTOM_RIGHT
        topRightNode.AddConnection(topMiddleNode);
        topRightNode.AddConnection(bottomRightNode);

        // BOTTOM_LEFT connects to: CENTER, TOP_LEFT, BOTTOM_RIGHT
        bottomLeftNode.AddConnection(centerNode);
        bottomLeftNode.AddConnection(topLeftNode);
        bottomLeftNode.AddConnection(bottomRightNode);

        // BOTTOM_RIGHT connects to: CENTER, TOP_RIGHT, BOTTOM_LEFT
        bottomRightNode.AddConnection(centerNode);
        bottomRightNode.AddConnection(topRightNode);
        bottomRightNode.AddConnection(bottomLeftNode);
    }

    private void AddNodeToMap(Node node, string id)
    {
        if (node != null)
        {
            node.nodeId = id;
            nodeMap[id] = node;
        }
        else
        {
            Debug.LogWarning($"Node {id} is not assigned in NodeManager!");
        }
    }

    // Get a node by its ID string
    public Node GetNodeById(string id)
    {
        if (nodeMap.TryGetValue(id, out Node node))
        {
            return node;
        }
        Debug.LogWarning($"Node with ID {id} not found!");
        return null;
    }


    // Get all nodes in the game
    public List<Node> GetAllNodes()
    {
        return new List<Node>(nodeMap.Values);
    }

    // Check if there's a valid path between two nodes
    public bool HasPath(Node from, Node to)
    {
        if (from == to) return true;
        if (from == null || to == null) return false;

        HashSet<Node> visited = new HashSet<Node>();
        return DFS(from, to, visited);
    }

    private bool DFS(Node current, Node target, HashSet<Node> visited)
    {
        if (current == target) return true;
        if (visited.Contains(current)) return false;

        visited.Add(current);

        foreach (Node neighbor in current.GetConnectedNodes())
        {
            if (DFS(neighbor, target, visited))
            {
                return true;
            }
        }

        return false;
    }


    // Find shortest path between two nodes using BFS
    // Returns list of nodes from source to destination
    public List<Node> FindShortestPath(Node from, Node to)
    {
        if (from == to)
        {
            return new List<Node> { from };
        }

        if (from == null || to == null)
        {
            return new List<Node>();
        }

        Queue<Node> queue = new Queue<Node>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();
        HashSet<Node> visited = new HashSet<Node>();

        queue.Enqueue(from);
        visited.Add(from);
        parent[from] = null;

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current == to)
            {
                // Reconstruct path
                List<Node> path = new List<Node>();
                Node node = to;
                while (node != null)
                {
                    path.Insert(0, node);
                    parent.TryGetValue(node, out node);
                }
                return path;
            }

            foreach (Node neighbor in current.GetConnectedNodes())
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    parent[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return new List<Node>();
    }
}