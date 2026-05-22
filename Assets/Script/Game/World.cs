using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public ItemData tempo;
    [Header("Global Parameter")]
    public GameObject player;
    public GameObject[] Enemy;
    public GameObject[] XpOrb;
    public ItemData[] item;
    public Transform monCanvas;
    public int Enemytest = 0;
    public float DificultyAugmentation;
    [Header("Spawn Parameter")]
    public float MinRadius;
    public float MaxRadius;
    private int SpawnRate = 5;
    private int MaxEnemy = 5000;
    private float RefreshTime = 5f;
    private float timer;
    private float timerUpdate;
    private Player playerScript;
    public List<GameObject> enemyList = new List<GameObject>();
    public List<GameObject> orbList = new List<GameObject>();
    private BasicEnemy EnemyScript;

    [Header("Réglage XP")]
    public float baseXP = 5f;
    public float exponent = 1.5f;

    //PARTICULE
    private ParticleHandler particleHandler;

    // TIMER
    [Header("Réglage Timer et dificulté")]
    public float timeLeft;
    public float DificultyIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyList.Clear();
        if (GameData.LevelDuration <= 0)
            GameData.LevelDuration = 600;
        playerScript = player.GetComponent<Player>();
        for (int i = 0; i < Enemytest; i++)
        {
            GameObject newEnemy = Instantiate(Enemy[0], new Vector3(4, 4, 0), Quaternion.identity);
            enemyList.Add(newEnemy);
            EnemyScript = newEnemy.GetComponent<BasicEnemy>();
            EnemyScript.PlayerObj = player;
            EnemyScript.WorldObj = this.gameObject;
        } 
        timeLeft = GameData.LevelDuration;
        DificultyIndex = Enemy.Length;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        timerUpdate += Time.deltaTime;
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            playerScript.UiScript.DisplayTimer(timeLeft);
        }
        if (timer > RefreshTime)
        {
            Spawner();
            timer = 0f;
        }
        if (timerUpdate >= GameData.updateInterval)
        {
            EnemyHandler();
            timerUpdate = 0f;
        }
    }

    void EnemyHandler()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null) 
                continue;
            Vector3 direction = player.transform.position - enemyList[i].transform.position;
            EnemyScript = enemyList[i].GetComponent<BasicEnemy>();
            enemyList[i].transform.position += direction.normalized * EnemyScript.enemyData.MovementSpeed * GameData.updateInterval;
        }

        for (int i = 0; i < orbList.Count; i++)
        {
            Vector3 direction = player.transform.position - orbList[i].transform.position;
            if (direction.magnitude <= playerScript.playerData.CollectDistance && direction.magnitude > 1)
                orbList[i].transform.position += direction.normalized * 10 * GameData.updateInterval;
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
            Vector3 hitPosition = enemy.transform.position;
            Vector3 hitDirection = player.transform.position - enemy.transform.position;
            ParticleManager.Instance.PlayEffect(ParticleEffectType.EnemyDeath, hitPosition, hitDirection);
            if (Random.Range(0, 2) != 0)
            {
                Vector3 spawnPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1.4f);
                GameObject newOrb = Instantiate(XpOrb[0], spawnPos, Quaternion.identity);
                orbList.Add(newOrb);
            }
            playerScript.EnemyKill++;   
            playerScript.UiScript.DisplayNumberKill(playerScript.EnemyKill);
            Destroy(enemy);     
        }
    }

    public void EndGame()
    {
        //Debug.Log("tu es mort");
    }

    public void Spawner()
    {
        float tmp = GameData.LevelDuration / DificultyIndex;
        float index = DificultyIndex - 1;
        float index2 = timeLeft;
        while (tmp < index2)
        {
            index2 -= tmp;
            index--;
        }
        if (index < 0)
            index = 0;
        for (int i = 0; i <= SpawnRate; i++)
        {
            if (MaxEnemy > enemyList.Count)
            {
                OneSpawnRadius(MinRadius, MaxRadius, Enemy[(int)index]);
            }
        }
    }

    void OneSpawnRadius(float min,float max, GameObject enemy)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(min, max);
        Vector3 spawnPosition = player.transform.position + new Vector3(randomDirection.x, randomDirection.y, 0) * randomDistance;
        GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        enemyList.Add(newEnemy);
        EnemyScript = newEnemy.GetComponent<BasicEnemy>();
        EnemyScript.PlayerObj = player;
        EnemyScript.WorldObj = this.gameObject;
    }

    public GameObject FindClosestEnemy(GameObject gameObj, GameObject ignore)
    {
        GameObject closest = null;
        float shortestDistanceSqr = Mathf.Infinity;
        Vector3 currentPos = gameObj.transform.position;

        foreach (GameObject enemy in enemyList)
        {
            if (enemy == ignore || enemy == null) 
                continue; 

            Vector3 diff = enemy.transform.position - currentPos;
            float curDistanceSqr = diff.sqrMagnitude;

            if (curDistanceSqr < shortestDistanceSqr)
            {
                closest = enemy;
                shortestDistanceSqr = curDistanceSqr;
            }
        }
        return closest;
    }
    public GameObject FindClosestEnemyList(GameObject gameObj, List<GameObject> BlackList)
    {
        GameObject closest = null;
        float shortestDistanceSqr = Mathf.Infinity;
        Vector3 currentPos = gameObj.transform.position;
        foreach (GameObject enemy in enemyList)
        {
            if (enemy == null)
                continue;
            bool shouldIgnored = false;
            foreach(GameObject ignore in BlackList)
            {
                if (enemy == ignore)
                {
                    shouldIgnored = true;
                    break;
                }
            }
            if (shouldIgnored)
                continue;

            Vector3 diff = enemy.transform.position - currentPos;
            float curDistanceSqr = diff.sqrMagnitude;

            if (curDistanceSqr < shortestDistanceSqr)
            {
                closest = enemy;
                shortestDistanceSqr = curDistanceSqr;
            }
        }
        return closest;
    }

    public void Temp()
    {
        // Vector3 spawnPosition = new Vector3(4, 4, 0);
        // GameObject enemy = Enemy[0];
        // GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        // enemyList.Add(newEnemy);
        // EnemyScript = newEnemy.GetComponent<BasicEnemy>();
        // EnemyScript.PlayerObj = player;
        // EnemyScript.WorldObj = this.gameObject;
        // playerScript.UiScript.SetRarityLoot();
        playerScript.UiScript.SetLevelUpBTn();
    }

    
    void UnSpawnDist()
    {
    }

}
