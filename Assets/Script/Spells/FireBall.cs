using UnityEngine;
using System.Collections.Generic;

public class FireBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ItemData item;
    public float timer;
    public GameObject WorldObj;
    public GameObject FireBallProjectilesObj;
    private Spells spellScrypt;
    private UIManager uiScript;
    private World worldScript;
    private float BounceLeft;
    public List<GameObject> AmmoList = new List<GameObject>();

    void Start()
    {
        WorldObj = GameObject.Find("World");
        spellScrypt = WorldObj.GetComponent<Spells>();
        uiScript = WorldObj.GetComponent<UIManager>();
        worldScript = WorldObj.GetComponent<World>();
        item = uiScript.newSpell;
        FireBallProjectilesObj = spellScrypt.AmmoPrefab[0];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > item.AttackSpeedBase * item.AttackSpeed && worldScript.enemyList.Count > 0)
        {
            SpellStart();
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

    void SpellStart()
    {
        for (int i = 0; i < item.ProjectileNumber; i++)
        {
            GameObject newAmmo = Instantiate(FireBallProjectilesObj, transform.position, Quaternion.identity);
            AmmoList.Add(newAmmo);
            FireBallProjectiles ammoScript = newAmmo.GetComponent<FireBallProjectiles>();
            ammoScript.FireBallWeapon = this.gameObject;
            ammoScript.BounceLeft = item.Bounce;
            GameObject target = worldScript.FindClosestEnemy(worldScript.player, null);
            if (!target)
            {
                Destroy(newAmmo);
                break;
            }
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();
            Rigidbody2D rb = newAmmo.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * item.Speed;
        }
    }
}
