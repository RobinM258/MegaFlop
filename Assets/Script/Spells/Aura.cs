using UnityEngine;

public class Aura : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ItemData item;
    public float timer;
    public GameObject WorldObj;
    private Spells spellScrypt;
    private UIManager uiScript;

    void Start()
    {
        WorldObj = GameObject.Find("World");
        Spells spellScrypt = WorldObj.GetComponent<Spells>();
        UIManager uiScript = WorldObj.GetComponent<UIManager>();
        item = uiScript.newSpell;
        SpellStart();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    void SpellStart()
    {
        transform.localScale = new Vector3(item.Size, item.Size, item.Size);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (timer > item.AttackSpeedBase / item.AttackSpeed)
            {
                BasicEnemy target = other.gameObject.GetComponent<BasicEnemy>();
                target.GetDamage(item.Damage);
                timer = 0f;
            }
        }
    }
}
