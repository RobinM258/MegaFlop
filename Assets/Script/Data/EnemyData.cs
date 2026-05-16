using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NouvelEnenemi", menuName = "MegaFlop/EnemyData")]
public class Enemyata : ScriptableObject
{
    public float Health;
    public float Damage;
    public float Level;
    public float MovementSpeed;
    public bool IsBoss;
    public bool IsQuest;
}
