using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NouveauPlayer", menuName = "MegaFlop/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float Health;
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

    public List<ItemData> PassifWeaponsList = new List<ItemData>();
    public List<ItemData> ActifWeaponsList = new List<ItemData>();
    public List<UpgradeData> UpgradeList = new List<UpgradeData>();
    public List<ItemData> Item = new List<ItemData>();
}
