using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private GridManager gridManager;

    public Node startNode; // Noeud de départ (pour debug)
    public Node targetNode; // Noeud d'arrivée (pour debug)
    public List<Node> visitedNodes = new List<Node>(); // Noeuds visités
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

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || (openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            visitedNodes.Add(currentNode); // Ajoute aux nœuds visités

            // Si on atteint la destination
            if (currentNode == targetNode)
            {
                finalPath = RetracePath(startNode, targetNode);
                return finalPath;
            }

            foreach (Node neighbour in gridManager.GetNeighbours(currentNode))
            {
                if (!neighbour.isWalkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                float newGCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newGCost < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newGCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
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
