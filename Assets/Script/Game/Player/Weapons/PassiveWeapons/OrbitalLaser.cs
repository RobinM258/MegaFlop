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
    public ItemData item;
    private Spells spellScrypt;
    public GameObject OrbitalLaserProjectil;
    private UIManager uiScript;
    public GameObject WorldObj;
    private World worldScript;
    
    public List<SlotData> ammoList = new List<SlotData>();
    
    public List<GameObject> enemyBlackList = new List<GameObject>();

    void Start()
    {
        WorldObj = GameObject.Find("World");
        spellScrypt = WorldObj.GetComponent<Spells>();
        uiScript = WorldObj.GetComponent<UIManager>();
        worldScript = WorldObj.GetComponent<World>();
        item = uiScript.newSpell;
        OrbitalLaserProjectil = spellScrypt.AmmoPrefab[1];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > item.AttackSpeedBase * item.AttackSpeed && worldScript.enemyList.Count > 0)
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
                    enemy.GetDamage(item.Damage);
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
