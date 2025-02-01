using UnityEngine;
using System.Collections.Generic;

public class ObjectMover : MonoBehaviour
{
    public Transform target; // La destination finale

    private GridManager gridManager;
    private List<Node> path;

    //grid position
    public Vector2Int gridPosition;

    void Start()
    {
        // Récupérer GridManager et Pathfinding
        gridManager = FindFirstObjectByType<GridManager>();
        Pathfinding pathfinding = FindFirstObjectByType<Pathfinding>();

        // Placer l'objet au node le plus proche
        MoveToNearestNode();

        // Calculer le chemin
        if (target != null)
        {
            path = pathfinding.FindPath(transform.position, target.position);
        }

        // Get position from grid
        gridPosition = gridManager.GetGridPositionFromWorldPosition(transform.position);

    }

    void Update()
    {
        // Déplacement le long du chemin
        if (path != null && path.Count > 0)
        {
            Node nextNode = path[0];
            transform.position = Vector3.MoveTowards(transform.position, nextNode.worldPosition, Time.deltaTime * 5f);

            // Si le node suivant est atteint, passer au suivant
            if (Vector3.Distance(transform.position, nextNode.worldPosition) < 0.1f)
            {
                path.RemoveAt(0);
            }
        }

        gridPosition = gridManager.GetGridPositionFromWorldPosition(transform.position);
    }

    public void MoveToNearestNode()
    {
        Node nearestNode = gridManager.GetNodeFromWorldPosition(transform.position);
        transform.position = nearestNode.worldPosition;
    }
}
