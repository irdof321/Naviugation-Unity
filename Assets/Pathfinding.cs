using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private GridManager gridManager;

    public Node startNode; // Noeud de départ (pour debug)
    public Node targetNode; // Noeud d'arrivée (pour debug)
    public PriorityQueue<Node> visitedNodes = new PriorityQueue<Node>(); // Noeuds à explorer
    public List<Node> finalPath = new List<Node>(); // Chemin final trouvé

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
    }

    public List<Node> FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        startNode = gridManager.GetNodeFromWorldPosition(startWorldPos);
        targetNode = gridManager.GetNodeFromWorldPosition(targetWorldPos);

        List<Node> openList = new List<Node>(); // Nodes à explorer
        HashSet<Node> closedList = new HashSet<Node>(); // Nodes déjà explorés
        visitedNodes.Clear(); // Réinitialiser la liste des nœuds visités
        finalPath.Clear(); // Réinitialiser le chemin final

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);
        visitedNodes.Enqueue(startNode, startNode.fCost);


        while (visitedNodes.Count > 0)
        {
            Node currentNode = visitedNodes.Dequeue();

            if (currentNode == targetNode)
            {
                finalPath = RetracePath(startNode, targetNode);
                return finalPath;
            }

            closedList.Add(currentNode);

            foreach (Node neighbour in gridManager.GetNeighbours(currentNode))
            {
                if (!neighbour.isWalkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                float g_tenative = currentNode.gCost + GetDistance(currentNode, neighbour);
                float h_tenative = GetDistance(neighbour, targetNode);
                float f_tenative = g_tenative + h_tenative;

                if (visitedNodes.Contains(neighbour) && g_tenative >= neighbour.gCost)
                {
                    continue;
                }
                {
                    continue;
                }
            }
        }

        return null; // Pas de chemin trouvé
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    private float GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int distY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        return distX + distY; // Distance de Manhattan
    }

    void OnDrawGizmos()
    {
        // Colorier le noeud de départ
        if (startNode != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(startNode.worldPosition, Vector3.one * (gridManager.nodeSize - 0.1f));
        }

        // Colorier le noeud d'arrivée
        if (targetNode != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(targetNode.worldPosition, Vector3.one * (gridManager.nodeSize - 0.1f));
        }

        // Colorier les noeuds visités
        Gizmos.color = new Color(1, 0, 0, 0.5f); // Rouge semi-transparent
        foreach (Node visited in visitedNodes)
        {
            Gizmos.DrawCube(visited.worldPosition, Vector3.one * (gridManager.nodeSize - 0.1f));
        }

        // Colorier le chemin final
        Gizmos.color = Color.green;
        foreach (Node pathNode in finalPath)
        {
            Gizmos.DrawCube(pathNode.worldPosition, Vector3.one * (gridManager.nodeSize - 0.1f));
        }
    }
}
