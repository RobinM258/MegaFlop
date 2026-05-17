using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Player Stats")]
    public GameObject[] Pannels;
    public TMP_Text FPSCount;
    public int CurrentSlotId;

    [Header("World Stats")]
    private float deltaTime = 0;
    private float deltaTimeFPS = 0;

    [Header("Level UP parameter")]
    public GameObject[] LevelUpBtn;
    public GameObject[] StatsBtn;
    public GameObject[] HotBarSlot;
    public GameObject[] PassiveWeaponBar;
    public GameObject[] UpgradeBar;
    public GameObject CurrentHotBarSlot;
    public GameObject WorldObj;
    public GameObject PlayerObj;

    private World WorldScript;
    private Player PlayerScript;
    private Spells SpellScript;


    public ItemData[] tempoItem;
    public ItemData newSpell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameData.isPaused = false;
        Time.timeScale = 1f;
    }
    void Start()
    {
        WorldScript = WorldObj.GetComponent<World>();
        PlayerScript = PlayerObj.GetComponent<Player>();
        SpellScript = WorldObj.GetComponent<Spells>();
        if (PlayerScript.playerData.PassiveWeaponsList.Count > 0)
            newSpell = PlayerScript.playerData.PassiveWeaponsList[0];
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        deltaTimeFPS += deltaTime;
        float fps = 1.0f / deltaTime;
        
        if (deltaTimeFPS >= 1)
        {
            FPSCount.text = string.Format("{0:0.} FPS", fps);
            deltaTimeFPS = 0;
        }

        Vector2 scrollValue = Mouse.current.scroll.ReadValue();
    
        if (scrollValue.y != 0)
        {
            HotBarManagerWheel((int)scrollValue.y);
        }
    }

    public void SwitchGame()
    {
        GameData.isPaused = !GameData.isPaused;
        Time.timeScale = GameData.isPaused ? 0f : 1f;
    }
    public void OnCancel(InputValue value)
    {
        GameData.isPaused = !GameData.isPaused;
        
        Pannels[0].SetActive(GameData.isPaused);

        Time.timeScale = GameData.isPaused ? 0f : 1f;
    }

    // Si pause true le jeu est en pause
    public void OpenPannel(int id, bool pause)
    {
        if (pause)
            SwitchGame();
        Pannels[id].SetActive(true);
    }

    // Si all  est true ferme tout les pannel
    public void ClosePannel(int id, bool all)
    {
        if (all)
        {
            for (int i = 0; i <= Pannels.Length; i++)
            {
                Pannels[i].SetActive(false);
            }
        }
        else
            Pannels[id].SetActive(false);
        if (Time.timeScale == 0f)
            SwitchGame();
    }

    // retourne  true si un pannel est ouvert
    public bool CheckPannel()
    {
        for (int i = 0; i < Pannels.Length; i++)
        {
            if (Pannels[i].activeSelf)
                return true;
        }
        return false;
    }

    public void SetDisplayXpLeft()
    {
        TMP_Text XPCount = StatsBtn[0].GetComponentInChildren<TMP_Text>();
        
        string strToDisplay = "XP : " + PlayerScript.playerData.Xp + " / " + PlayerScript.GetXPRequired(PlayerScript.playerData.Level);
        XPCount.text = strToDisplay;
    }

    public void DisplayPlayerHealth(float Health, float MaxHealth)
    {
        TMP_Text Heal = StatsBtn[1].GetComponentInChildren<TMP_Text>();
        string strToDisplay = Health.ToString("0") + " / " + MaxHealth.ToString("0");
        Heal.text = strToDisplay;
    }

    public void DisplayNumberKill(float number)
    {
        TMP_Text Kills = StatsBtn[2].GetComponentInChildren<TMP_Text>();
        string strToDisplay = number.ToString("0");
        Kills.text = strToDisplay;
    }

    public void DisplayTimer(float timeToDisplay)
    {
        TMP_Text TimeText = StatsBtn[3].GetComponentInChildren<TMP_Text>();
        if(timeToDisplay < 0)
            timeToDisplay = 0;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // public bool SetUpgrade()
    // {
    //     List<string> statWeapon = new List<string> { "Damage", "AttackSpeed", "Size", "PersonalCrit", "PersonalCritMult", "Bounce" };
    //     //List<string> statUpgrade = new List<string> { "Damage", "AttackSpeed", "Size" };
    //     int rarity = Random(10000, 20000) * PlayerScript.Chance;
    //     bool HasWeapons = false;
    //     bool HasUpgrade = false;
    //     if (PlayerScript.weaponsList.Count > 0)
    //         HasWeapons = true;
    //     if (PlayerScript.upgradeList.Count > 0)
    //         HasUpgrade = true;
    //     int rdm = 2;
    //     if (HasUpgrade && HasWeapons)
    //         rdm = Random(0, 1);
    //     if (rdm = 2)
    //         return false;
    //     else if (HasUpgrade)
    //     {
    //         rdm = Random(0, PlayerScript.upgradeList.Count);
            
    //     }
    //     else
    //     {
    //         rdm = Random(0, PlayerScript.weaponList.Count);
    //         ItemData weapon = PlayerScript.weaponList[rdm];
    //         string chosenStat = statNames[Random.Range(0, statNames.Count)];
    //         switch (chosenStat)
    //         {
    //             case "Damage":
    //                 weapon.Damage += 1f * (rarity / 10000 );
    //                 break;
    //             case "AttackSpeed":
    //                 weapon.AttackSpeed += 0.2f * (rarity / 10000);
    //                 break;
    //             case "Size":
    //                 weapon.Size += 0.1f * (rarity / 10000);
    //                 break;
    //             case "PersonalCrit":
    //                 weapon.PersonalCrit += 2f * (rarity / 5000);
    //                 break;
    //             case "PersonalCritMult":
    //                 weapon.PersonalCritMult += 2f * (rarity / 5000);
    //                 break;
    //             case "Bounce":
    //                 weapon.Bounce += 0.5 * (rarity / 10000);
    //                 break;
    //         }
    //     }
    //     return true;
    // }
    public void SetLevelUpBTn()
    {
        for (int i = 0; i < LevelUpBtn.Length; i++)
        {
            int rdm = Random.Range(0, WorldScript.item.Length);
            // foreach (ItemData weapon in PlayerScript.weaponsList)
            // {
            //     if (WorldScript.item[rdm] == weapon)
            //         Debug.Log("Tu l'as dejà, on va te proposer une ameilloration");
            //     else 
            //         tempoItem[i] = WorldScript.item[rdm];
            // }
            tempoItem[i] = WorldScript.item[i];
            LevelUpBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = WorldScript.item[i].itemName;
        }
    }

    public void LevelUpBtnId(int id)
    {
        newSpell = tempoItem[id];
        SpellScript.SetPassifWeapon(id);
        PlayerScript.playerData.PassiveWeaponsList.Add(newSpell);
        // ON pourra enlelver se if quand j'aurais refais le syteme de loot au hasard 
        if (PlayerScript.playerData.PassiveWeaponsList.Count <= 4)
        {
            DisplayPassiveWeapon(newSpell, PlayerScript.playerData.PassiveWeaponsList.Count - 1);
        }
        ClosePannel(1, false);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("MenuSelection");
    }

    //HotBar

    public void HotBarManagerWheel(int number)
    {
        int MaxSlot = PlayerScript.playerData.ActiveWeaponsList.Count - 1;
        int Index = CurrentSlotId + number;
        if (!GameData.isPaused)
        {
            if (Index < 0)
                CurrentSlotId = MaxSlot;
            else if (Index > MaxSlot)
                CurrentSlotId = 0;
            else
                CurrentSlotId = Index;
            if (PlayerScript.playerData.ActiveWeaponsList.Count - 1 > 0)
                CurrentHotBarSlot.transform.position = HotBarSlot[CurrentSlotId].transform.position;
        }
    }
    public void HotBarManagerKey(int number)
    {
        if (GameData.isPaused)
            CurrentSlotId = number;
    }

    public void DisplayPassiveWeapon(ItemData item, int id)
    {
        Image imageDuSlot = PassiveWeaponBar[id].GetComponent<Image>();
        imageDuSlot.sprite = item.icon;
    }

    public void DiplayUpgradeBar(ItemData item, int id)
    {
        Image imageDuSlot = UpgradeBar[PlayerScript.playerData.UpgradeList.Count - 1].GetComponent<Image>();
        imageDuSlot.sprite = item.icon;
    }
}
