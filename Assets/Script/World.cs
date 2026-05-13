using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    [Header("Global Parameter")]
    public GameObject player;
    public GameObject[] Enemy;
    public GameObject[] XpOrb;
    public ItemData[] item;
    public ItemData[] itemRemains;

    public int Enemytest = 0;
    [Header("Dificulty Parameter")]
    public float Dificulty;
    public float DificultyMultiplier;
    public float DificultyAugmentation;
    [Header("Spawn Parameter")]
    public float MinRadius;
    public float MaxRadius;
    private int SpawnRate;
    private int MaxEnemy = 1000;
    private float RefreshTime = 5.0f;
    private float timer;
    private Player playerScript;
    public List<GameObject> enemyList = new List<GameObject>();
    public List<GameObject> orbList = new List<GameObject>();
    private BasicEnemy EnemyScript;

    [Header("Réglage XP")]
    public float baseXP = 5f;
    public float exponent = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemData[] itemRemains = item;
        playerScript = player.GetComponent<Player>();
        SpawnRate = 3;
        for (int i = 0; i < Enemytest; i++)
        {
            GameObject newEnemy = Instantiate(Enemy[0], new Vector3(4, 4, 0), Quaternion.identity);
            enemyList.Add(newEnemy);
            EnemyScript = newEnemy.GetComponent<BasicEnemy>();
            EnemyScript.PlayerObj = player;
            EnemyScript.WorldObj = this.gameObject;
        } 
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > RefreshTime)
        {
            Spawner();
            timer = 0f;
        }
        EnemyHandler();
    }

    void EnemyHandler()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            Vector3 direction = player.transform.position - enemyList[i].transform.position;
            enemyList[i].transform.position += direction.normalized * EnemyScript.MoveSpeed * Time.deltaTime;
        }

        for (int i = 0; i < orbList.Count; i++)
        {
            Vector3 direction = player.transform.position - orbList[i].transform.position;
            if (direction.magnitude <= playerScript.CollectDistance && direction.magnitude > 1)
                orbList[i].transform.position += direction.normalized * 10 * Time.deltaTime;
            else if (direction.magnitude <= 1)
            {
                GameObject current = orbList[i];
                XpOrb xp = current.GetComponent<XpOrb>();
                playerScript.AddXp(xp.xpValue);
                orbList.Remove(current);
                Destroy(current);
            }
        }
    }

    public void KillEnemy(GameObject enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
            if (Random.Range(0, 2) != 0)
            {
                Vector3 spawnPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1.4f);
                GameObject newOrb = Instantiate(XpOrb[0], spawnPos, Quaternion.identity);
                orbList.Add(newOrb);
            }
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
        for (int i = 0; i <= SpawnRate; i++)
        {
            if (MaxEnemy > enemyList.Count)
            {
                OneSpawnRadius(MinRadius, MaxRadius, Enemy[0]);
            }
        }
    }

    void OneSpawnRadius(float min,float max, GameObject enemy)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(min, max);
        Vector3 spawnPosition = player.transform.position + new Vector3(randomDirection.x, randomDirection.y, 0) * randomDistance;
        GameObject newEnemy = Instantiate(Enemy[0], spawnPosition, Quaternion.identity);
        enemyList.Add(newEnemy);
        EnemyScript = newEnemy.GetComponent<BasicEnemy>();
        EnemyScript.PlayerObj = player;
        EnemyScript.WorldObj = this.gameObject;
    }
    
    void UnSpawnDist()
    {

    }



    //SPELL

    public GameObject[] WeaponsPrefab;
    public void SetAura()
    {
        GameObject nouvelObjet = Instantiate(WeaponsPrefab[0], player.transform);
        nouvelObjet.transform.SetParent(player.transform);
        nouvelObjet.transform.localPosition = new Vector3(0, 0, 1.5f);
    }
}
