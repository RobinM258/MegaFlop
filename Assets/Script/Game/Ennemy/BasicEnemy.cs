using UnityEngine;
using System.Collections.Generic;

public class BasicEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public EnemyData enemyData;
    private EnemyData myStats;

    [Header("Global Parameter")]
    public GameObject PlayerObj;
    public GameObject WorldObj;
    private World WorldScript;
    public GameObject damagePopupPrefab;
    public Transform SC_DamageIndicatorList;  

    [Header("Grid Collision System")]
    public int currentCellID;
    public Vector2Int currentCoords;
    private int lastCellID = -1;
    [SerializeField] private float repulsionRadius = 1.5f; 
    [SerializeField] private float repulsionForce = 2f;    

    void Start()
    {
        EnemyData instanceData = ScriptableObject.CreateInstance<EnemyData>();
        instanceData.CopyFrom(enemyData); 
        myStats = instanceData;
        
        WorldScript = WorldObj.GetComponent<World>();
        SC_DamageIndicatorList = WorldScript.SC_DamageIndicatorList;
        
        RegisterInGrid();
    }

    void Update()
    {
        if (CollisionController.Test == null || PlayerObj == null) return;

        // 1. MISE À JOUR DE LA GRILLE SPATIALE
        currentCellID = CollisionController.Test.GetCellIndexFromPosition(transform.position);
        if (currentCellID != lastCellID)
        {
            CollisionController.Test.UpdateEntityPosition(this.gameObject, lastCellID, currentCellID);
            lastCellID = currentCellID;
        }
        currentCoords = CollisionController.Test.GetCellCoordsFromPosition(transform.position);

        // 2. CALCUL DU MOUVEMENT FUSIONNÉ
        CalculerMouvementGlobal();
    }
    
    private void CalculerMouvementGlobal()
    {
        // --- FORCE A : Direction vers le joueur ---
        Vector3 directionToPlayer = (PlayerObj.transform.position - transform.position).normalized;
        Vector3 forceJoueur = directionToPlayer * myStats.MovementSpeed;

        // --- FORCE B : Évitement des voisins (Répulsion) ---
        List<GameObject> neighbors = CollisionController.Test.GetNearbyEntities(transform.position);
        Vector3 forceRepulsion = Vector3.zero;

        foreach (GameObject other in neighbors)
        {
            if (other == this.gameObject || other == null) continue;

            float distance = Vector2.Distance(transform.position, other.transform.position);

            if (distance < repulsionRadius && distance > 0.01f)
            {
                Vector3 directionAway = transform.position - other.transform.position;
                float strength = (repulsionRadius - distance) / repulsionRadius;
                forceRepulsion += directionAway.normalized * strength;
            }
        }
        forceRepulsion *= repulsionForce;

        // --- FUSION ET BRIDAGE DE LA VITESSE ---
        Vector3 mouvementFinal = forceJoueur + forceRepulsion;

        // OPTIMISATION : On s'assure que le monstre ne dépasse JAMAIS sa vitesse max,
        // même s'il est très poussé par ses voisins.
        if (mouvementFinal.magnitude > myStats.MovementSpeed)
        {
            mouvementFinal = mouvementFinal.normalized * myStats.MovementSpeed;
        }

        // On applique le déplacement avec Time.deltaTime
        transform.position += mouvementFinal * Time.deltaTime;
    }
    
    public void GetDamage(float damage, bool crit)
    {
        Vector3 spawnPosition = transform.position;
        GameObject popupGo = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity, SC_DamageIndicatorList);
        DamageIndicator popupScript = popupGo.GetComponent<DamageIndicator>();
        popupScript.Setup(damage, crit);
        
        if (damage >= myStats.Health)
            WorldScript.KillEnemy(this.gameObject);
        else
            myStats.Health -= damage;
    }

    private void RegisterInGrid()
    {
        if (CollisionController.Test != null)
        {
            lastCellID = CollisionController.Test.GetCellIndexFromPosition(transform.position);
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