using UnityEngine;

public class Node
{
    public Vector3 worldPosition; // Position 3D du node
    public Vector2Int gridPosition; // Indices (x, y) dans la grille
    public bool isWalkable; // Est-ce qu’on peut marcher ici ?

    public float gCost; // Coût entre le point de départ et ce node
    public float hCost; // Coût estimé entre ce node et la destination
    public float fCost => gCost + hCost; // Somme de gCost et hCost
    public Node parent; // Référence vers le node précédent dans le chemin
    

    public Node(Vector3 worldPos, Vector2Int gridPos, bool walkable)
    {
        worldPosition = worldPos;
        gridPosition = gridPos;
        isWalkable = walkable;

        gCost = float.MaxValue; // Initialement, très grand pour indiquer qu’il n’a pas encore été visité
        hCost = 0;
        parent = null;
    }
}

