using UnityEngine;


[CreateAssetMenu(fileName = "NouvelItem", menuName = "MegaFlop/Item")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public string itemType;
    public Sprite icon;
    public string description;
    public int Level;
    public float Damage;
    public float Speed;
    public float TotalDamage;
    public float AttackSpeedBase;
    public float AttackSpeed;
    public float CritChance;
    public float CritMult;
    public float Size;
    public float Bounce;
    public float ProjectileNumber;

    public void CopyFrom(ItemData other)
    {
        this.id = other.id;
        this.itemName = other.itemName;
        this.itemType = other.itemType;
        this.icon = other.icon;
        this.description = other.description;
        this.Level = other.Level;
        this.Damage = other.Damage;
        this.Speed = other.Speed;
        this.TotalDamage = other.TotalDamage;
        this.AttackSpeedBase = other.AttackSpeedBase;
        this.AttackSpeed = other.AttackSpeed;
        this.CritChance = other.CritChance;
        this.CritMult = other.CritMult;
        this.Size = other.Size;
        this.Bounce = other.Bounce;
        this.ProjectileNumber = other.ProjectileNumber;
    }
    public ItemData() {}
}