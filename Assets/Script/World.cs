using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    [Header("Global Parameter")]
    public GameObject Player;
    public GameObject[] Enemy;

    public int Enemytest = 0;
    [Header("Dificulty Parameter")]
    public float Dificulty;
    public float DificultyMultiplier;
    [Header("Spawn Parameter")]
    public float MinRadius;
    public float MaxRadius;
    private float SpawnRate;
    //private int MaxEnemy = 10;


    public List<GameObject> enemyList = new List<GameObject>();
    private BasicEnemy EnemyScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Enemytest; i++)
        {
            GameObject newEnemy = Instantiate(Enemy[0], new Vector3(4, 4, 0), Quaternion.identity);
            enemyList.Add(newEnemy);
            EnemyScript = newEnemy.GetComponent<BasicEnemy>();
            EnemyScript.PlayerObj = Player;
            EnemyScript.WorldObj = this.gameObject;
        } 
    }

    void Update()
    {
        EnemyHandler();
    }

    void EnemyHandler()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            Vector3 direction = Player.transform.position - enemyList[i].transform.position;
            enemyList[i].transform.position += direction.normalized * EnemyScript.MoveSpeed * Time.deltaTime;
        }
    }

    public void KillEnemy(GameObject enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
            Destroy(enemy);
        }
    }

    public void EndGame()
    {
        Debug.Log("tu es mort");
    }

    public void Spawner()
    {
        float RealDificulty = Dificulty * DificultyMultiplier;
        // if (RealDificulty >= 4)
        //     Debug.Log("Tu gagnes une épée !");
        // if (RealDificulty >= 3)
        //     Debug.Log("Tu gagnes un bouclier !");
        if (RealDificulty >= 2)
            Debug.Log("Tu gagnes une potion !");
        else
        {
            OneSpawnRadius(MinRadius, MaxRadius, Enemy[0]);
        }
            Debug.Log("Tu ne gagnes rien !");   
    }

    void OneSpawnRadius(float min,float max, GameObject enemy)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(min, max);
        Vector3 spawnPosition = Player.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;
        GameObject newEnemy = Instantiate(Enemy[0], spawnPosition, Quaternion.identity);
        enemyList.Add(newEnemy);
        EnemyScript = newEnemy.GetComponent<BasicEnemy>();
        EnemyScript.PlayerObj = Player;
        EnemyScript.WorldObj = this.gameObject;
    }
    
    void UnSpawnDist()
    {

    }
}
