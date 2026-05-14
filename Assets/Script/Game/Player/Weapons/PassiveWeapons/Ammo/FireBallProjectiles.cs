using UnityEngine;

public class FireBallProjectiles : MonoBehaviour
{
    public GameObject FireBallWeapon;
    private FireBall FireBallScript;
    public float BounceLeft;

    private GameObject lastTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FireBallScript = FireBallWeapon.GetComponent<FireBall>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckBounce()
    {
        if (BounceLeft <= 0)
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
            target.GetDamage(FireBallScript.item.Damage);
            BounceLeft--;
            CheckBounce();
        }
        else if (other.CompareTag("Tree"))
        {
            lastTarget = other.gameObject;
            Debug.Log("Arbre");
            BounceLeft--;
            CheckBounce();
        }
    }  
}
