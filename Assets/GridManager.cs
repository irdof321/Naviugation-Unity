using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public Vector2 gridSize; // Taille de la grille (en unités)
    public float nodeSize; // Taille d’un node (en unités)
    private Node[,] grid; // La grille logique

    public Node[,] Grid => grid; // Propriété publique pour accéder à la grille

    private Vector3 gridOrigin; // Point d'origine pour centrer la grille

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        int gridCountX = Mathf.RoundToInt(gridSize.x / nodeSize);
        int gridCountY = Mathf.RoundToInt(gridSize.y / nodeSize);

        grid = new Node[gridCountX, gridCountY];

        // Calcul de l'origine de la grille pour qu'elle soit centrée
        gridOrigin = new Vector3(-gridSize.x / 2, 0, -gridSize.y / 2);

        for (int x = 0; x < gridCountX; x++)
        {
            for (int y = 0; y < gridCountY; y++)
            {
                // Calcul de la position du node
                Vector3 worldPos = gridOrigin + new Vector3(x * nodeSize + nodeSize / 2, 0, y * nodeSize + nodeSize / 2);

                // Vérifier si un obstacle est présent
                bool walkable = !Physics.CheckSphere(worldPos, nodeSize / 2);

                // Créer le node
                grid[x, y] = new Node(worldPos, new Vector2Int(x, y), walkable);
            }
        }
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        // Convertir la position du monde en position de grille
        float percentX = Mathf.Clamp01((worldPosition.x - gridOrigin.x) / gridSize.x);
        float percentY = Mathf.Clamp01((worldPosition.z - gridOrigin.z) / gridSize.y);

        int x = Mathf.FloorToInt(percentX * grid.GetLength(0));
        int y = Mathf.FloorToInt(percentY * grid.GetLength(1));

        return grid[x, y];
    }

    public Vector2Int GetGridPositionFromWorldPosition(Vector3 worldPosition)
    {
        // Convertir la position du monde en indices de grille
        float percentX = Mathf.Clamp01((worldPosition.x - gridOrigin.x) / gridSize.x);
        float percentY = Mathf.Clamp01((worldPosition.z - gridOrigin.z) / gridSize.y);

        int x = Mathf.FloorToInt(percentX * grid.GetLength(0));
        int y = Mathf.FloorToInt(percentY * grid.GetLength(1));

        return new Vector2Int(x, y);
    }

    public Node[] GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        int x = node.gridPosition.x;
        int y = node.gridPosition.y;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {


                int checkX = x + dx;
                int checkY = y + dy;

                if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours.ToArray();
    }

    void OnDrawGizmos()
    {
        if (grid == null) return;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Node node = grid[x, y];
                Gizmos.color = node.isWalkable ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f); // Couleurs transparentes
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeSize - 0.1f));
            }
        }
    }
}