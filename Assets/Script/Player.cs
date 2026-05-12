using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Player Stats")]
    public ItemData[] item;
    public float Health;
    public float xp;
    public float Level;
    public float MoveSpeed;
    public float CritPercent;
    public float CritMultiplier;
    public float AttaqueSpeed;
    public float Armor;
    public float Chance;
    public float VulnerabilityTime;
    public float Thorns;
    public float CollectDistance;
    public float PlayerX;
    public float PlayerY;

    [Header("Global Parameter")]
    public TMP_Text HealCount;
    public GameObject WorldObj;


    private Vector2 direction;
    private World WorldScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealCount.text = Health.ToString();
        WorldScript = WorldObj.GetComponent<World>();

    }

    // Update is called once per frame
    void Update()
    {
        HealCount.text = Health.ToString();
        Vector3 deplacement = new Vector3(direction.x, direction.y, 0);
        transform.position += deplacement * MoveSpeed * Time.deltaTime;
        PlayerX = transform.position.x;
        PlayerY = transform.position.y;
    }

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    public void OnCancel(InputValue value)
    {
        UIManager ui = WorldObj.GetComponent<UIManager>();
        
        if (ui != null)
        {
            if (!ui.CheckPannel() && !ui.Pannels[0].activeSelf)
                ui.OpenPannel(0, true);
            else if (ui.Pannels[0].activeSelf)
                ui.ClosePannel(0, false);
        }
    }

    public void OnTab(InputValue value)
    {
        WorldScript.Spawner();
    }

    public void GetDamage(float damage)
    {
        if (Health <= 1)
            WorldScript.EndGame();
        else 
        {
            if (damage <= Armor)
                Health--;
            else if (damage - Armor >= Health)
                WorldScript.EndGame();
            else
                Health = Health - (damage - Armor);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            BasicEnemy EnemyScript = collision.gameObject.GetComponent<BasicEnemy>();
            GetDamage(EnemyScript.Damage);
            if (Thorns > 0)
                EnemyScript.GetDamage(Thorns);
        }
    }

    public void AddXp(float nb)
    {
        xp += nb;
        while (xp >= GetXPRequired(Level))
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        UIManager ui = WorldObj.GetComponent<UIManager>();
        xp -= GetXPRequired(Level);
        ui.SetLevelUpBTn();
        ui.OpenPannel(1, true);
        Level++;
    }
    public float GetXPRequired(float level)
    {
        return Mathf.Round(WorldScript.baseXP * Mathf.Pow(level, WorldScript.exponent));
    }

}
