using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BasicEnemy : MonoBehaviour
{

    [Header("Enemy Stats")]
    public EnemyData enemyData;
    private EnemyData myStats;
    [Header("Global Parameter")]
    public GameObject PlayerObj;
    public GameObject WorldObj;
    private World WorldScript;


    //COLISION
    public int currentCellID;
    public Vector2Int currentCoords;
    private int lastCellID = -1;
    public int enemyinCell = 0;
    void Start()
    {
        EnemyData instanceData = ScriptableObject.CreateInstance<EnemyData>();
        instanceData.CopyFrom(enemyData); 
        myStats = instanceData;
        WorldScript = WorldObj.GetComponent<World>();
        // On s'assure que l'ennemi est enregistré dans la grille dès le départ
        RegisterInGrid();
    }

    void Update()
    {
        // On ne calcule l'ID de la case qu'une fois par frame
        int currentCellID = CollisionController.Test.GetCellIndexFromPosition(transform.position);

        // OPTIMISATION : On ne prévient le manager que si on a changé de case !
        if (currentCellID != lastCellID)
        {
            CollisionController.Test.UpdateEntityPosition(this.gameObject, lastCellID, currentCellID);
            lastCellID = currentCellID;
        }
    }

    void UpdateSpatialPosition()
    {
        if (CollisionController.Test == null) return;

        // 1. On récupère l'ID unique (utile pour ton ancien système de voisins)
        currentCellID = CollisionController.Test.GetCellIndexFromPosition(transform.position);

        // 2. On récupère les coordonnées X (colonne) et Y (ligne)
        currentCoords = CollisionController.Test.GetCellCoordsFromPosition(transform.position);
    }
    
    public void GetDamage(float damage)
    {
        if (damage >= myStats.Health)
            WorldScript.KillEnemy(this.gameObject);
        else
            myStats.Health = myStats.Health - damage;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(transform.position, 0.7f); 
    //     #if UNITY_EDITOR
    //     UnityEditor.Handles.Label(transform.position + Vector3.up, 
    //         $"Case: {currentCellID}\nCoords: ({currentCoords.x}, {currentCoords.y})");
    //     #endif
    // }

    private void RegisterInGrid()
    {
        if (CollisionController.Test != null)
        {
            lastCellID = CollisionController.Test.GetCellIndexFromPosition(transform.position);
            // On passe -1 en 'oldCell' car il n'était nulle part avant
            CollisionController.Test.UpdateEntityPosition(this.gameObject, -1, lastCellID);
        }
    }

    private void OnDestroy()
    {
        if (CollisionController.Test != null)
        {
            CollisionController.Test.UpdateEntityPosition(this.gameObject, lastCellID, -1);
        }
    }
}
