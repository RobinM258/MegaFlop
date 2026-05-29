using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine.EventSystems;

[System.Serializable]
public struct UpgradeData
{
    public string Name;
    public string Stats;
    public string Rarity;
    public float Value;

    // Un constructeur pour créer l'objet facilement en une ligne
    public UpgradeData(string name, string stats, string rarity, float value)
    {
        Name = name;
        Stats = stats;
        Rarity = rarity;
        Value = value;
    }
}

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
    public GameObject[] ModuleBar;
    public GameObject CurrentHotBarSlot;
    public GameObject WorldObj;
    public GameObject PlayerObj;

    private World WorldScript;
    private Player PlayerScript;
    private Spells SpellScript;
    public List<ItemData> weaponRemain = new List<ItemData>();
    public List<ItemData> upgradeRemain = new List<ItemData>();


    public ItemData[] tempoItem;
    public ItemData newItem;

    //Upgrade

    private int statsToUpgrade;
    private string rarity;
    private string nameStats;
    public Image[] bordureBtn;
    public List<UpgradeData> UpgradeDataList = new List<UpgradeData>();


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
            newItem = PlayerScript.playerData.PassiveWeaponsList[0];
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
        
        string strToDisplay = "XP : " + PlayerScript.playerData.Souls + " / " + PlayerScript.GetXPRequired(PlayerScript.playerData.Level);
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
    // NOMBRE DE RARETE 5 (Commun, Peu Commun, Rare, Epique, Legendaire)

    public int SetRarityLoot()
    {
        int rdm = Random.Range(0, 101);
        switch (rdm)
        {
            case int n when (n >= 0 && n < 51):
                rarity = "Commun";
                return 1;
            case int n when (n >= 51 && n < 76):
                rarity = "Peu Commun";
                return 2;
            case int n when (n >= 76 && n < 91):
                rarity = "Rare";
                return 3;
            case int n when (n >= 91 && n < 97):
                rarity = "Epique";
                return 4;
            case int n when (n >= 91 && n < 97):
                rarity = "Légendaire";
                return 5;   
        }
        return -1;
    }
    ItemData GetItemRef(string name)
    {
        ItemData item;
        ItemData[] PassiveWeaponItemRef = SpellScript.PassiveWeaponItem;
        ItemData[] UpgradeItemRef = SpellScript.UpgradeItem;

        for (int i = 0; i < PassiveWeaponItemRef.Length; i++)
        {
            if(PassiveWeaponItemRef[i].name == name)
            {
                item = PassiveWeaponItemRef[i];
                return item;
            }
        }
        for (int i = 0; i < UpgradeItemRef.Length; i++)
        {
            Debug.Log(UpgradeItemRef[i].name + " | " + name);
            if(UpgradeItemRef[i].name == name)
            {
                item = UpgradeItemRef[i];
                return item;
            }
        }
        return null;
    }

    List<float> GetItemStats(ItemData item)
    {
        List<float> RetList = new List<float>();
        System.Type typeOfItem = item.GetType();
        FieldInfo[] variables = typeOfItem.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo champ in variables)
        {  
            object valeurDeLaVariable = champ.GetValue(item); 
            System.Type typeDeLaVariable = champ.FieldType;

            if (valeurDeLaVariable == null)
                continue;
            if (typeDeLaVariable == typeof(float))
                RetList.Add((float)valeurDeLaVariable);
        }
        return RetList;
    }

    float SetRarityFromStats(string statsName, ItemData item)
    {
        float upStats = 0;
        int rdm = SetRarityLoot();
        ItemData itemRef = GetItemRef(item.itemName);
        System.Type typeOfItem = itemRef.GetType();
        FieldInfo[] variables = typeOfItem.GetFields(BindingFlags.Public | BindingFlags.Instance);
         foreach (FieldInfo champ in variables)
        {  
            if (champ.Name == statsName)
            {
                object valeurDeLaVariable = champ.GetValue(itemRef); 
                float value = 0;
                if (champ.Name == "PersonalCrit" || champ.Name == "PersonnalCritMult")
                    value = 50;
                else 
                    value = (float)valeurDeLaVariable;
                upStats = value * rdm / 10;
                if (upStats < 0)
                    upStats =  upStats * -1f;
                Debug.Log("up stats = " + upStats);
            }
        }
        return upStats;
    }

    public float SetUpgradeWeapon(ItemData item)
    {
        List<string> weaponStats = new List<string> { "Damage", "Speed", "AttackSpeed", "Size", "PersonalCrit", "PersonnalCritMult", "Bounce", "ProjectileNumber" };
        System.Type typeOfItem = item.GetType();
        FieldInfo[] variables = typeOfItem.GetFields(BindingFlags.Public | BindingFlags.Instance);
        int index = 0;
        foreach (FieldInfo champ in variables)
        {
            if (weaponStats.Contains(champ.Name))
            {      
                object valeurDeLaVariable = champ.GetValue(item); 
                System.Type typeDeLaVariable = champ.FieldType;

                bool GoodValue = false;
                if (valeurDeLaVariable == null)
                    continue;
                if (typeDeLaVariable == typeof(float))
                {
                    float value = (float)valeurDeLaVariable;
                    if (value > 0)
                        GoodValue = true;
                }
                else if (typeDeLaVariable == typeof(int))
                {
                    int value = (int)valeurDeLaVariable;
                    if (value > 0)
                        GoodValue = true;
                }
                if (GoodValue)
                    index++;
            }
        }
        int rdm = Random.Range(0, index);
        index = 0;
        foreach (FieldInfo champ in variables)
        {
            if (weaponStats.Contains(champ.Name))
            {
                string nomDeLaVariable = champ.Name;           
                object valeurDeLaVariable = champ.GetValue(item); 
                System.Type typeDeLaVariable = champ.FieldType;

                bool GoodValue = false;
                if (valeurDeLaVariable == null)
                    continue;
                if (typeDeLaVariable == typeof(float))
                {
                    float value = (float)valeurDeLaVariable;
                    if (value > 0)
                        GoodValue = true;
                }
                else if (typeDeLaVariable == typeof(int))
                {
                    int value = (int)valeurDeLaVariable;
                    if (value > 0)
                        GoodValue = true;
                }
                if (GoodValue && rdm == index)
                {  
                    nameStats = nomDeLaVariable;
                    float up = SetRarityFromStats(nomDeLaVariable, item);
                    return up;
                }
                else if (GoodValue)
                    index++;
            }
        }
        return 1f;
    }

    public void GetItemRemains()
    {
        weaponRemain.Clear();
        upgradeRemain.Clear();
        foreach (ItemData weapon in SpellScript.PassiveWeaponItem)
        {
            if (!PlayerScript.playerData.PassiveWeaponsList.Contains(weapon))
                weaponRemain.Add(weapon);
        }
        foreach (ItemData upgrade in SpellScript.UpgradeItem)
        {
            if (!PlayerScript.playerData.UpgradeList.Contains(upgrade))
                upgradeRemain.Add(upgrade);
        }
    }

    public ItemData SetRandomItem(List<ItemData> itemList, ItemData[] blackList)
    {
        List<ItemData> itemsAllowed = new List<ItemData>();

        foreach (ItemData item in itemList)
        {
            if (blackList != null && !blackList.Contains(item))
                itemsAllowed.Add(item);
        }
        if (itemsAllowed.Count == 0)
        {
        
            return null;
        }
        int rdm = Random.Range(0, itemsAllowed.Count);
        return itemsAllowed[rdm];    
    }

    public ItemData GetRandomPlayerItem()
    {
        List<ItemData> itemsAllowed = new List<ItemData>();
        int rdm;
        foreach (ItemData item in PlayerScript.playerData.PassiveWeaponsList)
        {
            itemsAllowed.Add(item);
        }
        foreach (ItemData item in PlayerScript.playerData.UpgradeList)
        {
            itemsAllowed.Add(item);
        }
        rdm = Random.Range(0, itemsAllowed.Count);
        ItemData itemSelectionne = itemsAllowed[rdm];
        return itemSelectionne; 
    }

    public void SetLevelUpBTn()
    {
        int rdm;
        float value;
        ItemData[] choixPris = new ItemData[3];
        ItemData itemToUpgrade;
        bool canTakeWeapon = false;
        bool canTakeUpgrade = false;
        
        UpgradeDataList.Clear();
        GetItemRemains();
        if (PlayerScript.playerData.PassiveWeaponsList.Count < 4)
        {
            if (weaponRemain.Count > 0)
                canTakeWeapon = true;
        }
        if (PlayerScript.playerData.UpgradeList.Count < 4)
        {
            if (upgradeRemain.Count > 0)
                canTakeUpgrade = true;
        }
        for (int i = 0; i < LevelUpBtn.Length; i++)
        {
            List<string> choixPossibles = new List<string>();
            Button bouton = LevelUpBtn[i].GetComponent<Button>();
            ColorBlock cb = bouton.colors;
            cb.normalColor = Color.gray;

            if (canTakeWeapon && weaponRemain.Count > 0) 
                choixPossibles.Add("Arme");
            if (canTakeUpgrade && upgradeRemain.Count > 0) 
                choixPossibles.Add("Module");
            choixPossibles.Add("Upgrade");
            choixPossibles.Add("Upgrade");
            int indexChoix = Random.Range(0, choixPossibles.Count);
            string choixSelectionne = choixPossibles[indexChoix];
            if (choixSelectionne == "Arme")
            {
                cb.normalColor = Color.gray;
                tempoItem[i] = SetRandomItem(weaponRemain, tempoItem);
                weaponRemain.Remove(tempoItem[i]);
                UpgradeDataList.Add(new UpgradeData("Arme", "", "", 0f));
                if (tempoItem[i] == null)
                    Debug.Log("Il y a un bug");
            }
            else if (choixSelectionne == "Module")
            {
                cb.normalColor = Color.gray;
                tempoItem[i] = SetRandomItem(upgradeRemain, tempoItem);
                upgradeRemain.Remove(tempoItem[i]);
                UpgradeDataList.Add(new UpgradeData("module", "", "", 0f));
                if (tempoItem[i] == null)
                    Debug.Log("Il y a un bug");
            }
            else if (choixSelectionne == "Upgrade")
            {
                tempoItem[i] = null;
                if (PlayerScript.playerData.PassiveWeaponsList.Count > 0 && PlayerScript.playerData.UpgradeList.Count == 0)
                {
                    itemToUpgrade = SetRandomItem(PlayerScript.playerData.PassiveWeaponsList, choixPris);
                }
                else if (PlayerScript.playerData.UpgradeList.Count > 0 && PlayerScript.playerData.PassiveWeaponsList.Count == 0)
                {
                    itemToUpgrade = SetRandomItem(PlayerScript.playerData.UpgradeList, choixPris);
                }
                else
                {
                    rdm = Random.Range(0, 2);
                    if (rdm == 0)
                        itemToUpgrade = SetRandomItem(PlayerScript.playerData.PassiveWeaponsList, choixPris);
                    else
                        itemToUpgrade = SetRandomItem(PlayerScript.playerData.UpgradeList, choixPris);
                }
                if (itemToUpgrade)
                {
                    choixPris[i] = itemToUpgrade;
                    value = SetUpgradeWeapon(itemToUpgrade);
                    UpgradeDataList.Add(new UpgradeData(itemToUpgrade.itemName, nameStats, rarity, value));
                    float displayValue = value * 100;

                    // if (displayValue < 0)
                    //     displayValue = displayValue * -1;
                    // string displayString = $"{displayValue:F0}%";
                    string DisplayBtn = "Upgrade " + itemToUpgrade.itemName  + "\n" + nameStats + " + " + value;
                    switch (rarity)
                        {
                        case "Commun":
                            cb.normalColor = Color.gray;
                            break;
                        case "Peu Commun":
                            cb.normalColor = Color.green;
                            break;

                        case "Rare":
                            cb.normalColor = Color.blue;
                            break;

                        case "Epique":
                            cb.normalColor = Color.magenta;
                            break;

                        case "Légendaire":
                            cb.normalColor = Color.yellow;
                            break;
                    }                    
                    LevelUpBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = DisplayBtn;
                }
                else
                {
                    i--;
                    continue;
                }
            }
            bouton.colors = cb;
            if (tempoItem[i] != null)
                LevelUpBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = tempoItem[i].itemName;
        }
    }

    public void LevelUpBtnId(int id)
    {
        EventSystem.current.SetSelectedGameObject(null);
        newItem = tempoItem[id];
        //Upgrade choisi
        if (!newItem)
        {
            int index = 0;
            foreach(ItemStats stats in SpellScript.itemstats)
            {
                if (stats.item.itemName == UpgradeDataList[id].Name)
                {
                    System.Type typeOfItem = stats.item.GetType();
                    FieldInfo[] variables = typeOfItem.GetFields(BindingFlags.Public | BindingFlags.Instance);
                    foreach(FieldInfo champ in variables)
                    {
                        string varName = champ.Name;
                        if (nameStats == varName)
                        {    
                            object valeurDeLaVariable = champ.GetValue(stats.item); 
                            System.Type typeDeLaVariable = champ.FieldType;
                            if (typeDeLaVariable == typeof(float))
                            {
                                float value = (float)valeurDeLaVariable;
                                value += (UpgradeDataList[id].Value);
                                champ.SetValue(stats.item, value);
                                if (varName == "Size")
                                {
                                    stats.RefreshSize();
                                }
                            }
                        }
                        index++;
                    }
                }
            }
        }
        else if (newItem.itemType == "Passive")
        {
            SpellScript.SetPassiveWeapon(newItem.id);
            PlayerScript.playerData.PassiveWeaponsList.Add(newItem);

            DisplayPassiveWeapon(newItem, PlayerScript.playerData.PassiveWeaponsList.Count - 1);
        }
        else if (newItem.itemType == "Module")
        {
            PlayerScript.playerData.UpgradeList.Add(newItem);
            DisplayUpgradeBar(newItem);
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

    public void DisplayUpgradeBar(ItemData item)
    {
        Image imageDuSlot = ModuleBar[PlayerScript.playerData.UpgradeList.Count - 1].GetComponent<Image>();
        imageDuSlot.sprite = item.icon;
    }
}
