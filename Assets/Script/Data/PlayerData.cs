using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NouveauPlayer", menuName = "MegaFlop/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float Health;
    public float MaxHealth;
    public float Xp;
    public float Gold;
    public float Level;
    public float MoveSpeed;
    public float CritPercent;
    public float CritMultiplier;
    public float AttaqueSpeed;
    public float Armor;
    public float Chance;
    public float InvulnerabilityTime;
    public float Thorns;
    public float CollectDistance;

    public List<ItemData> PassiveWeaponsList = new List<ItemData>();
    public List<ItemData> ActiveWeaponsList = new List<ItemData>();
    public List<ItemData> UpgradeList = new List<ItemData>();
    public List<ItemData> Item = new List<ItemData>();

    public void CopyFrom(PlayerData other)
    {
        this.Health = other.Health;
        this.MaxHealth = other.MaxHealth;
        this.Xp = other.Xp;
        this.Gold = other.Gold;
        this.Level = other.Level;
        this.MoveSpeed = other.MoveSpeed;
        this.CritPercent = other.CritPercent;
        this.CritMultiplier = other.CritMultiplier;
        this.AttaqueSpeed = other.AttaqueSpeed;
        this.Armor = other.Armor;
        this.Armor = other.Armor;
        this.Chance = other.Chance;
        this.InvulnerabilityTime = other.InvulnerabilityTime;
        this.Thorns = other.Thorns;
        this.CollectDistance = other.CollectDistance;

        this.PassiveWeaponsList = new List<ItemData>(other.PassiveWeaponsList);
        this.ActiveWeaponsList = new List<ItemData>(other.ActiveWeaponsList);
        this.UpgradeList = new List<ItemData>(other.UpgradeList);
        this.Item = new List<ItemData>(other.Item);
    }
    public PlayerData() {}
}
