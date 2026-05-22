using UnityEngine;

public class Aura : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float timer;
    public GameObject WorldObj;
    private Spells spellScrypt;
    private UIManager uiScript;
    private ItemStats itemStats;

    void Start()
    {
        itemStats = GetComponent<ItemStats>();
        WorldObj = GameObject.Find("World");
        Spells spellScrypt = WorldObj.GetComponent<Spells>();
        UIManager uiScript = WorldObj.GetComponent<UIManager>();
        itemStats.item = uiScript.newItem;;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void RefreshSize()
    {
        transform.localScale = new Vector3(itemStats.item.Size, itemStats.item.Size, itemStats.item.Size);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (timer > itemStats.item.AttackSpeedBase / itemStats.item.AttackSpeed)
            {
                BasicEnemy target = other.gameObject.GetComponent<BasicEnemy>();
                target.GetDamage(itemStats.item.Damage);
                timer = 0f;
            }
        }
    }
}
