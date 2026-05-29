using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SlotData
{
    public GameObject target;
    public float time;
}

public class OrbitalLaser : MonoBehaviour
{
    public float timer;
    private Spells spellScript;
    public GameObject OrbitalLaserProjectil;
    private UIManager uiScript;
    public GameObject WorldObj;
    private World worldScript;
    
    public List<SlotData> ammoList = new List<SlotData>();
    public List<GameObject> enemyBlackList = new List<GameObject>();
    private ItemStats itemStats;
    private ItemData item;


    private float Damage;
    private float AttackSpeed;

    void Start()
    {
        itemStats = GetComponent<ItemStats>();
        item = itemStats.item;
        WorldObj = GameObject.Find("World");
        spellScript = WorldObj.GetComponent<Spells>();
        uiScript = WorldObj.GetComponent<UIManager>();
        worldScript = WorldObj.GetComponent<World>();
        OrbitalLaserProjectil = spellScript.AmmoPrefab[1];
    }

    void Update()
    {
        timer += Time.deltaTime;
        AttackSpeed = itemStats.item.AttackSpeed +  spellScript.PlayerScript.GetItemStat("AttackSpeed", spellScript.PlayerScript.playerData.UpgradeList);
        if (timer > item.AttackSpeedBase / AttackSpeed && worldScript.enemyList.Count > 0)
        {
            SpellStart();
            timer = 0;
        }
    }

    void SpellStart()
    {
        float now = Time.time;
    
        for (int i = ammoList.Count - 1; i >= 0; i--)
        {
            SlotData slot = ammoList[i];
    
            if (!slot.target)
            {
                enemyBlackList.Remove(slot.target);
                ammoList.RemoveAt(i);
                continue;
            }
    
            if (slot.time + 3f <= now)
            {
                BasicEnemy enemy = slot.target.GetComponent<BasicEnemy>();
                if (enemy != null)
                {
                    Damage = itemStats.item.Damage + spellScript.PlayerScript.GetItemStat("Damage", spellScript.PlayerScript.playerData.UpgradeList);
                    enemy.GetDamage(Damage);
                }
    
                enemyBlackList.Remove(slot.target);
    
                Transform ammo = slot.target.transform.Find("OrbitalLaserAmmo");
                if (ammo != null)
                {
                    Destroy(ammo.gameObject);
                }
    
                ammoList.RemoveAt(i);
            }
        }
    
        for (int i = 0; i < item.ProjectileNumber; i++)
        {
            GameObject target = worldScript.FindClosestEnemyList(worldScript.player, enemyBlackList);
    
            if (target)
            {
                enemyBlackList.Add(target);
    
                GameObject newAmmo = Instantiate(
                    OrbitalLaserProjectil,
                    target.transform.position,
                    Quaternion.identity
                );
    
                newAmmo.transform.SetParent(target.transform);
                newAmmo.name = "OrbitalLaserAmmo";
    
                ammoList.Add(new SlotData
                {
                    target = target,
                    time = Time.time
                });
            }
        }
    }
}
