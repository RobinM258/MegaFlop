using UnityEngine;

public class BasicEnemy : MonoBehaviour
{

    [Header("Enemy Stats")]
    public float Health;
    public float MoveSpeed;
    public float Damage;

    [Header("Global Parameter")]
    public GameObject PlayerObj;
    public GameObject WorldObj;

    private World WorldScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WorldScript = WorldObj.GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void GetDamage(float damage)
    {
        if (damage >= Health)
            WorldScript.KillEnemy(this.gameObject);
        else
            Health = Health - damage;
    }
}
