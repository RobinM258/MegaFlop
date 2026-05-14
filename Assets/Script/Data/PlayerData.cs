using UnityEngine;


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

}
