using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    [Header("Player Stats")]
    public GameObject[] Pannels;
    public TMP_Text FPSCount;

    [Header("World Stats")]
    public bool isPaused = false;
    private float deltaTime = 0;
    private float deltaTimeFPS = 0;

    [Header("Level UP parameter")]
    public GameObject[] LevelUpBtn;

    public GameObject WorldObj;
    public GameObject PlayerObj;

    private World WorldScript;
    private Player PlayerScript;


    public ItemData[] tempoItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WorldScript = WorldObj.GetComponent<World>();
        PlayerScript = PlayerObj.GetComponent<Player>();
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
    }

    public void SwitchGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
    public void OnCancel(InputValue value)
    {
        isPaused = !isPaused;
        
        Pannels[0].SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
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
        Debug.Log("Pas de pannel");
        return false;
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
            int rdm = Random.Range(0, WorldScript.itemRemains.Length);
            // foreach (ItemData weapon in PlayerScript.weaponsList)
            // {
            //     if (WorldScript.item[rdm] == weapon)
            //         Debug.Log("Tu l'as dejà, on va te proposer une ameilloration");
            //     else 
            //         tempoItem[i] = WorldScript.item[rdm];
            // }
            tempoItem[i] = WorldScript.item[rdm];
            LevelUpBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = WorldScript.item[rdm].itemName;
        }
    }

    public void LevelUpBtnId(int id)
    {
        Debug.Log(id);
        WorldScript.SetAura();
        PlayerScript.weaponsList.Add(tempoItem[id]);
        ClosePannel(1, false);
    }
}
