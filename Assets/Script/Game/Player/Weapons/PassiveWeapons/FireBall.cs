using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FireBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float timer;
    public GameObject WorldObj;
    public GameObject FireBallProjectilesObj;
    public Spells spellScript;
    private UIManager uiScript;
    private World worldScript;
    private float BounceLeft;
    public ItemStats itemStats;
    public ItemData item;
    public List<GameObject> AmmoList = new List<GameObject>();
    private Transform SC_FireballList;

    private float AttackSpeed;
    private float Bounce;
    private float Speed;

    void Start()
    {
        itemStats = GetComponent<ItemStats>();
        item = itemStats.item;
        WorldObj = GameObject.Find("World");
        spellScript = WorldObj.GetComponent<Spells>();
        uiScript = WorldObj.GetComponent<UIManager>();
        worldScript = WorldObj.GetComponent<World>();
        FireBallProjectilesObj = spellScript.AmmoPrefab[0];
        SC_FireballList = worldScript.SC_FireballList;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        AttackSpeed = itemStats.item.AttackSpeed +  spellScript.PlayerScript.GetItemStat("AttackSpeed", spellScript.PlayerScript.playerData.UpgradeList);
        if (timer > item.AttackSpeedBase / AttackSpeed && worldScript.enemyList.Count > 0)
        {
            StartCoroutine(SpellStart());
            timer = 0;
        }
    }

    public void Newtarget(GameObject gameObj, GameObject enemy)
    {
        GameObject target = worldScript.FindClosestEnemy(gameObj, enemy);
        if (!target)
            Destroy(gameObj);
        else
        {
            Vector3 direction = target.transform.position - gameObj.transform.position;
            direction.Normalize();
            Rigidbody2D rb = gameObj.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * item.Speed;
        }
    }

    IEnumerator SpellStart()
    {
        for (int i = 0; i < item.ProjectileNumber; i++)
        {
            GameObject newAmmo = Instantiate(FireBallProjectilesObj, transform.position, Quaternion.identity, SC_FireballList);
            newAmmo.transform.localScale = new Vector3(item.Size, item.Size, item.Size);
            AmmoList.Add(newAmmo);
            FireBallProjectiles ammoScript = newAmmo.GetComponent<FireBallProjectiles>();
            ammoScript.FireBallWeapon = this.gameObject;
            Bounce = item.Bounce + spellScript.PlayerScript.GetItemStat("Bounce", spellScript.PlayerScript.playerData.UpgradeList);
            ammoScript.BounceLeft = Bounce;
            GameObject target = worldScript.FindClosestEnemy(worldScript.player, null);
            if (!target)
            {
                Destroy(newAmmo);
                yield break;
            }
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();
            Rigidbody2D rb = newAmmo.GetComponent<Rigidbody2D>();
            Speed = item.Speed + spellScript.PlayerScript.GetItemStat("Speed", spellScript.PlayerScript.playerData.UpgradeList);
            rb.linearVelocity = direction * Speed;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
