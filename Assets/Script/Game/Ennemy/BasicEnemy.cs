using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyData instanceData = ScriptableObject.CreateInstance<EnemyData>();
        instanceData.CopyFrom(enemyData); 
        myStats = instanceData;
        WorldScript = WorldObj.GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void GetDamage(float damage)
    {
        if (damage >= myStats.Health)
            WorldScript.KillEnemy(this.gameObject);
        else
            myStats.Health = myStats.Health - damage;
    }
}
