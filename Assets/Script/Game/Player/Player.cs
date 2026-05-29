using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Reflection;

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
        playerData.MaxBlood = 100;
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
                //TowerDetection();
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


    // IMPUT CONTROLLER
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
        LevelUp();
        //WorldScript.Spawner();
        if (playerData.ActiveWeaponsList.Count <= 3)
        {
            playerData.ActiveWeaponsList.Add(tempo);
            SpellScript.SetActifWeapon(playerData.ActiveWeaponsList[playerData.ActiveWeaponsList.Count - 1]);
        }
    }

    // GET

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

        public float GetXPRequired(float level)
    {
        return Mathf.Round(WorldScript.baseXP * Mathf.Pow(level, WorldScript.exponent));
    }

    public float GetItemStat(string name, List<ItemData> list)
    {
        float value = 0;
        foreach(ItemData module in list)
        {
            System.Type typeOfItem = module.GetType();
            FieldInfo[] variables = typeOfItem.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo champ in variables)
            {
                if (champ.Name == name)
                {
                    object valeurDeLaVariable = champ.GetValue(module); 
                    value = (float)valeurDeLaVariable;
                    if (value > 0)
                        return value;
                }
            }
        }
        return value;
    }

    public bool GetCrit(ItemData item)
    {
        float CritPercentage = 0;

        if (item)
            CritPercentage += item.CritChance;
        CritPercentage += playerData.CritChance;
        CritPercentage += GetItemStat("PersonalCrit", playerData.UpgradeList);
        if (CritPercentage == 0)
            return false;
        int rdm = Random.Range(0, 100);
    
        if (rdm < CritPercentage)
            return true;
        return false;
    }

    public float GetCritMult(ItemData item)
    {
        float CritMult = 0;

        if (item)
            CritMult += item.CritMult;
        CritMult += playerData.CritMultiplier;
        CritMult += GetItemStat("CritMult", playerData.UpgradeList);
        return CritMult;
    }

    //COLLISION
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && Invulnerability == false)
        {
            BasicEnemy EnemyScript = collision.gameObject.GetComponent<BasicEnemy>();
            GetDamage(EnemyScript.enemyData.Damage);
            if (playerData.Thorns > 0)
                EnemyScript.GetDamage(playerData.Thorns, false);
            Invulnerability = true;
        }
    }

    public void AddSouls(float nb)
    {
        playerData.Souls += nb;
        UiScript.SetDisplayXpLeft();
        while (playerData.Souls >= GetXPRequired(playerData.Level))
        {
            LevelUp();
        }
    }

    public void AddBlood(float nb)
    {
        if (playerData.Blood + nb > playerData.MaxBlood)
            playerData.Blood = playerData.MaxBlood;
        else 
            playerData.Blood += nb;   
    }
    public void LevelUp()
    {
        UIManager ui = WorldObj.GetComponent<UIManager>();
        playerData.Souls -= GetXPRequired(playerData.Level);
        ui.SetLevelUpBTn();
        ui.OpenPannel(1, true);
        playerData.Level++;
        UiScript.SetDisplayXpLeft();
    }

}
