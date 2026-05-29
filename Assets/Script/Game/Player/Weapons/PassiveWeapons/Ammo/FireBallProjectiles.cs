using UnityEngine;

public class FireBallProjectiles : MonoBehaviour
{
    public GameObject FireBallWeapon;
    private FireBall FireBallScript;
    public float BounceLeft;

    private GameObject lastTarget;
    private float Damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FireBallScript = FireBallWeapon.GetComponent<FireBall>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void RefreshSize()
    // {
    //     transform.localScale = new Vector3(item.Size, item.Size, item.Size);
    // }
    void CheckBounce()
    {
        if (BounceLeft < 1)
            Destroy(this.gameObject);
        else
            FireBallScript.Newtarget(this.gameObject, lastTarget);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            lastTarget = other.gameObject;
            BasicEnemy target = other.gameObject.GetComponent<BasicEnemy>();
            bool iscrit = FireBallScript.spellScript.PlayerScript.GetCrit(FireBallScript.itemStats.item);
            float critMult = 1f;
            if (iscrit)
                critMult = FireBallScript.spellScript.PlayerScript.GetCritMult(FireBallScript.itemStats.item);
            Damage = ((FireBallScript.itemStats.item.Damage + FireBallScript.spellScript.PlayerScript.GetItemStat("Damage", FireBallScript.spellScript.PlayerScript.playerData.UpgradeList)) * critMult);
            target.GetDamage(Damage, iscrit);
            BounceLeft--;
            CheckBounce();
        }
        else if (other.CompareTag("Tree"))
        {
            lastTarget = other.gameObject;
            Destroy(other.gameObject);
            BounceLeft--;
            CheckBounce();
        }
    }  
}
