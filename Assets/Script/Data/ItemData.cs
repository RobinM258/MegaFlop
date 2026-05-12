using UnityEngine;

[CreateAssetMenu(fileName = "NouvelItem", menuName = "MegaFlop/Item")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public string description;
    public int Level;
    public float Damage;
    public float TotalDamage;
    public float AttackSpeed;
    public float PersonalCrit;
    public float PersonnalCritMult;
    public float Size;
}