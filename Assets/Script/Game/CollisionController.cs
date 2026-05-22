using UnityEngine;
using System.Collections.Generic;

public class CollisionController : MonoBehaviour
{
public static CollisionController Test;

    [Header("Configuration de la Grille 2D")]
    [SerializeField] private Vector2 mapSize = new Vector2(20f, 20f); 
    [SerializeField] private Vector2 cellSize = new Vector2(2f, 2f); 
    public Dictionary<int, List<GameObject>> gridBuckets = new Dictionary<int, List<GameObject>>();
    private int lastCellID = -1;

    private int columns;
    private int rows;

    private void Awake()
    {
        if (Test == null) Test = this;
        CalculateGrid();
    }

    void Update()
    {
        LogicDeVoisinage();
    }

    private void CalculateGrid()
    {
        columns = Mathf.CeilToInt(mapSize.x / cellSize.x);
        rows = Mathf.CeilToInt(mapSize.y / cellSize.y);
    }

    public int GetCellIndexFromPosition(Vector2 worldPosition)
    {
        float localX = worldPosition.x + (mapSize.x / 2f) - transform.position.x;
        float localY = worldPosition.y + (mapSize.y / 2f) - transform.position.y;

        int col = Mathf.FloorToInt(localX / cellSize.x);
        int row = Mathf.FloorToInt(localY / cellSize.y);

        col = Mathf.Clamp(col, 0, columns - 1);
        row = Mathf.Clamp(row, 0, rows - 1);

        return row * columns + col;
    }

    // Le moteur de voisinage (Identique, mais utilise les colonnes calculées)
    public List<int> GetNeighbors(int currentCell)
    {
        List<int> neighbors = new List<int>() { currentCell };
        int col = currentCell % columns;
        int row = currentCell / columns;

        bool isLeft = (col == 0);
        bool isRight = (col == columns - 1);
        bool isTop = (row == rows - 1);
        bool isBottom = (row == 0);

        if (!isTop) neighbors.Add(currentCell + columns);
        if (!isBottom) neighbors.Add(currentCell - columns);
        if (!isLeft) neighbors.Add(currentCell - 1);
        if (!isRight) neighbors.Add(currentCell + 1);

        if (!isTop && !isRight) neighbors.Add(currentCell + columns + 1);
        if (!isTop && !isLeft) neighbors.Add(currentCell + columns - 1);
        if (!isBottom && !isRight) neighbors.Add(currentCell - columns + 1);
        if (!isBottom && !isLeft) neighbors.Add(currentCell - columns - 1);

        return neighbors;
    }

    public Vector2Int GetCellCoordsFromPosition(Vector2 worldPosition)
    {
        float localX = worldPosition.x + (mapSize.x / 2f) - transform.position.x;
        float localY = worldPosition.y + (mapSize.y / 2f) - transform.position.y;

        int col = Mathf.FloorToInt(localX / cellSize.x);
        int row = Mathf.FloorToInt(localY / cellSize.y);

        return new Vector2Int(
            Mathf.Clamp(col, 0, columns - 1), 
            Mathf.Clamp(row, 0, rows - 1)
        );
    }

    public void UpdateEntityPosition(GameObject entity, int oldCell, int newCell)
    {
        // On retire l'ennemi de son ancienne case
        if (gridBuckets.ContainsKey(oldCell))
            gridBuckets[oldCell].Remove(entity);

        // On l'ajoute dans la nouvelle
        if (!gridBuckets.ContainsKey(newCell))
            gridBuckets[newCell] = new List<GameObject>();

        gridBuckets[newCell].Add(entity);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Vector3 pos = transform.position;

    //     int previewCols = Mathf.CeilToInt(mapSize.x / cellSize.x);
    //     int previewRows = Mathf.CeilToInt(mapSize.y / cellSize.y);

    //     // Dessiner les lignes verticales (Axe X)
    //     for (int i = 0; i <= previewCols; i++)
    //     {
    //         float xOffset = (i * cellSize.x) - (mapSize.x / 2f);
    //         Vector3 start = pos + new Vector3(xOffset, -mapSize.y / 2f, 0);
    //         Vector3 end = pos + new Vector3(xOffset, mapSize.y / 2f, 0);
    //         Gizmos.DrawLine(start, end);
    //     }

    //     // Dessiner les lignes horizontales (Axe Y)
    //     for (int j = 0; j <= previewRows; j++)
    //     {
    //         float yOffset = (j * cellSize.y) - (mapSize.y / 2f);
    //         Vector3 start = pos + new Vector3(-mapSize.x / 2f, yOffset, 0);
    //         Vector3 end = pos + new Vector3(mapSize.x / 2f, yOffset, 0);
    //         Gizmos.DrawLine(start, end);
    //     }
    //     if (Application.isPlaying && gridBuckets != null)
    //     {
    //         foreach (var bucket in gridBuckets)
    //         {
    //             // On calcule la position du texte (centre de la case)
    //             int col = bucket.Key % columns;
    //             int row = bucket.Key / columns;
    //             Vector3 cellPos = transform.position + new Vector3(
    //                 (col * cellSize.x) - (mapSize.x / 2f) + (cellSize.x / 2f),
    //                 (row * cellSize.y) - (mapSize.y / 2f) + (cellSize.y / 2f),
    //                 0
    //             );
    //             UnityEditor.Handles.Label(cellPos, bucket.Value.Count.ToString());
    //         }
    //     }
    // }
    public List<GameObject> GetNearbyEntities(Vector2 worldPosition)
    {
        List<GameObject> nearbyEntities = new List<GameObject>();
        
        // 1. Trouver la case centrale
        int centerCell = GetCellIndexFromPosition(worldPosition);
        
        // 2. Récupérer les IDs de la case centrale + les 8 voisines
        List<int> cellsToCheck = GetNeighbors(centerCell);
        
        // 3. Récupérer tous les ennemis dans ces cases
        foreach (int cellID in cellsToCheck)
        {
            if (gridBuckets.TryGetValue(cellID, out List<GameObject> entitiesInCell))
            {
                nearbyEntities.AddRange(entitiesInCell);
            }
        }
        
        return nearbyEntities;
    }

    void LogicDeVoisinage()
    {
        List<GameObject> neighbors = CollisionController.Test.GetNearbyEntities(transform.position);
        //Debug.Log($"Entity {gameObject.name} has {neighbors.Count} neighbors in nearby cells.");
        Vector2 separationForce = Vector2.zero;

        foreach (GameObject other in neighbors)
        {
            if (other == this.gameObject) continue;

            float distance = Vector2.Distance(transform.position, other.transform.position);
            float repulsionRadius = 0.8f; // Distance à laquelle ils commencent à se pousser

            if (distance < repulsionRadius)
            {
                // On calcule un vecteur qui va de l'autre vers moi
                Vector2 directionAway = (Vector2)transform.position - (Vector2)other.transform.position;
                
                // Plus ils sont proches, plus la force est grande
                float strength = (repulsionRadius - distance) / repulsionRadius;
                separationForce += directionAway.normalized * strength;
            }
        }

        // On applique la force au mouvement (exemple simple)
        transform.position += (Vector3)separationForce * Time.deltaTime * 2f;
    }

    private void OnDestroy()
    {
        if (CollisionController.Test != null)
        {
            CollisionController.Test.UpdateEntityPosition(this.gameObject, lastCellID, -1);
        }
    }
}
