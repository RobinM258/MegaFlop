using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public GameObject Player;

    public int Enemytest = 0;
    public GameObject Enemy;

    public List<GameObject> enemyList = new List<GameObject>();
    private BasicEnemy EnemyScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Enemytest; i++)
        {
            GameObject newEnemy = Instantiate(Enemy, new Vector3(4, 4, 0), Quaternion.identity);
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
}
