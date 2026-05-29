using UnityEngine;
using System.Collections.Generic;

public class Aura : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float timer;
    public GameObject WorldObj;
    public Spells spellScript;
    private UIManager uiScript;
    private ItemStats itemStats;

    private List<GameObject>TargetInWeaponRange = new List<GameObject>();

    //private stats

    private float Damage;
    private float AttackSpeed;
    private float RotationSpeed  = 10f;
    void Start()
    {
        itemStats = GetComponent<ItemStats>();
        WorldObj = GameObject.Find("World");
        spellScript = WorldObj.GetComponent<Spells>();
        UIManager uiScript = WorldObj.GetComponent<UIManager>();
        Vector3 GoodPos = transform.position;
        GoodPos.y = -0.2f;
        transform.position = GoodPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        AttackSpeed = itemStats.item.AttackSpeed +  spellScript.PlayerScript.GetItemStat("AttackSpeed", spellScript.PlayerScript.playerData.UpgradeList);
        if (timer > itemStats.item.AttackSpeedBase / AttackSpeed)
        {
            for (int i = TargetInWeaponRange.Count - 1; i >= 0; i--)
            {
                GameObject target = TargetInWeaponRange[i];

                if (target == null)
                {
                    TargetInWeaponRange.RemoveAt(i);
                    continue;
                }
                BasicEnemy targetScript = target.GetComponent<BasicEnemy>();
                if (targetScript != null) 
                {
                    Damage = itemStats.item.Damage + spellScript.PlayerScript.GetItemStat("Damage", spellScript.PlayerScript.playerData.UpgradeList);
                    targetScript.GetDamage(Damage);
                }
            }
            timer = 0f;
        }
        transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
           TargetInWeaponRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
           TargetInWeaponRange.Remove(other.gameObject);
        }
    }
}
