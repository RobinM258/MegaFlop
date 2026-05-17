using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    [Header("Player Stats")]
    public ItemData tempo;
    public PlayerData DefaultPlayer;
    public bool Invulnerability;
    private float timer;
    public float EnemyKill;
    private WeaponManager weaponManagerScript;

    [Header("Global Parameter")]
    public GameObject WorldObj;
    public PlayerData playerData;
    private Vector2 direction;
    private World WorldScript;
    public UIManager UiScript;
    private Spells SpellScript;

    private Animator animator;

    [Header("Movement Smoothing")]
    public float AccelerationTime = 0.08f;
    public float DecelerationTime = 0.12f;

    private Vector2 currentVelocity;
    private Vector2 velocitySmoothing;

    //TOWER

    private float UpdateTimer;

    void Start()
    {
        if (GameData.PlayerSelected == null)
            GameData.PlayerSelected=  DefaultPlayer;
        PlayerData instanceData = ScriptableObject.CreateInstance<PlayerData>();
        instanceData.CopyFrom(GameData.PlayerSelected);
        playerData = instanceData;
        WorldScript = WorldObj.GetComponent<World>();
        UiScript = WorldObj.GetComponent<UIManager>();
        animator = GetComponent<Animator>();
        SpellScript = WorldObj.GetComponent<Spells>();
        Transform Enfant = transform.Find("ActiveWeaponManager");
        weaponManagerScript = Enfant.GetComponent<WeaponManager>();
        UiScript.DisplayPlayerHealth(playerData.Health, playerData.MaxHealth);
        UiScript.DisplayPassiveWeapon(playerData.PassiveWeaponsList[0], 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer = Time.deltaTime;
        if (Invulnerability)
        {
            timer += Time.deltaTime;
            if (timer >= playerData.InvulnerabilityTime)
            {
                Invulnerability = false;
                timer = 0;
            }
            if (UpdateTimer >= GameData.updateInterval)
            {
                TowerDetection();
                UpdateTimer = 0;  
            }
        }

        Vector2 targetVelocity = direction.normalized * playerData.MoveSpeed;
        float smoothTime = direction.sqrMagnitude > 0.001f ? AccelerationTime : DecelerationTime;
    
        currentVelocity = Vector2.SmoothDamp(
            currentVelocity,
            targetVelocity,
            ref velocitySmoothing,
            smoothTime
        );

        transform.position += (Vector3)(currentVelocity * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();

        bool isRunning = direction != Vector2.zero;
        animator.SetBool("IsRunning", isRunning);

        if (isRunning)
        {
            animator.SetFloat("XInput", direction.x);
            animator.SetFloat("YInput", direction.y);
        }
    }

    void OnLeftClick(InputValue value)
    {
        UIManager ui = WorldObj.GetComponent<UIManager>();

        if (value.isPressed)
        {
            // Time.timeScale
            if (playerData.ActiveWeaponsList.Count > 0 && GameData.isPaused == false)
            {
                weaponManagerScript.UseWeapon(playerData.ActiveWeaponsList[ui.CurrentSlotId]);
            }
        }
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
        if (playerData.ActiveWeaponsList.Count <= 3)
        {
            playerData.ActiveWeaponsList.Add(tempo);
            SpellScript.SetActifWeapon(playerData.ActiveWeaponsList[playerData.ActiveWeaponsList.Count - 1]);
        }
    }

    public void GetDamage(float damage)
    {
        if (playerData.Health <= 1)
            WorldScript.EndGame();
        else 
        {
            if (damage <= playerData.Armor)
                playerData.Health--;
            else if (damage - playerData.Armor >= playerData.Health)
            {
                playerData.Health = playerData.Health - (damage - playerData.Armor);
                UiScript.DisplayPlayerHealth(playerData.Health, playerData.MaxHealth);
                WorldScript.EndGame();
            }
            else
                playerData.Health = playerData.Health - (damage - playerData.Armor);
        }
        UiScript.DisplayPlayerHealth(playerData.Health, playerData.MaxHealth);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Invulnerability == false)
        {
            BasicEnemy EnemyScript = collision.gameObject.GetComponent<BasicEnemy>();
            GetDamage(EnemyScript.enemyData.Damage);
            if (playerData.Thorns > 0)
                EnemyScript.GetDamage(playerData.Thorns);
            Invulnerability = true;
        }
    }

    public void AddXp(float nb)
    {
        playerData.Xp += nb;
        UiScript.SetDisplayXpLeft();
        while (playerData.Xp >= GetXPRequired(playerData.Level))
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        UIManager ui = WorldObj.GetComponent<UIManager>();
        playerData.Xp -= GetXPRequired(playerData.Level);
        ui.SetLevelUpBTn();
        ui.OpenPannel(1, true);
        playerData.Level++;
        UiScript.SetDisplayXpLeft();
    }

    public float GetXPRequired(float level)
    {
        return Mathf.Round(WorldScript.baseXP * Mathf.Pow(level, WorldScript.exponent));
    }

    public void TowerDetection()
    {
        for (int i = 0; i < WorldScript.TowerInLevel.Length; i++)
        {
            Vector2 direction = WorldScript.TowerInLevel[i].transform.position - transform.position;
            float dist = direction.magnitude;
        }
    }

}
