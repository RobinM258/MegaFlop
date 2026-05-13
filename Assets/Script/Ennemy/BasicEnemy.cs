using UnityEngine;

using TMPro;

public class BasicEnemy : MonoBehaviour
{

    [Header("Enemy Stats")]
    public float Health;
    public float MoveSpeed;
    public float Damage;

    [Header("Global Parameter")]
    public GameObject PlayerObj;
    public GameObject WorldObj;
    public GameObject DamageCounter;
    public Transform monCanvas;

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
        // Vector3 screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
        // GameObject damagecount = Instantiate(DamageCounter, monCanvas);

        // damagecount.transform.position = screenPos;
        // TextMeshProUGUI texte = damagecount.GetComponent<TextMeshProUGUI>();
        // texte.text = damage.ToString();
        // Destroy(damagecount, 2f);
        if (damage >= Health)
            WorldScript.KillEnemy(this.gameObject);
        else
            Health = Health - damage;
    }
}
