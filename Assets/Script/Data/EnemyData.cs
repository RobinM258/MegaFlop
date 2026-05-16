using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NouvelEnenemi", menuName = "MegaFlop/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float Health;
    public float Damage;
    public float Level;
    public float MovementSpeed;
    public bool IsBoss;
    public bool IsQuest;

    public void CopyFrom(EnemyData other)
    {
        this.Health = other.Health;
        this.Damage = other.Damage;
        this.Level = other.Level;
        this.MovementSpeed = other.MovementSpeed;
        this.IsBoss = other.IsBoss;
        this.IsQuest = other.IsQuest;
    }
    public EnemyData() {}
}
