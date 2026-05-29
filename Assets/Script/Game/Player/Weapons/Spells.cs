using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Spells : MonoBehaviour
{

    public GameObject WorldObj;
    public GameObject PlayerObj;

    public Player PlayerScript;
    private WeaponManager weaponManagerScript;
    private UIManager UiScript;
    public GameObject[] PassiveWeaponPrefab;
    public GameObject[] AmmoPrefab;
    public ItemData[] PassiveWeaponItem;
    public ItemData[] UpgradeItem;

    [SerializeField]
    public List<ItemStats> itemstats = new List<ItemStats>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerScript = PlayerObj.GetComponent<Player>();
        Transform Enfant = PlayerObj.transform.Find("ActiveWeaponManager");
        weaponManagerScript = Enfant.GetComponent<WeaponManager>();
        if (PlayerScript.playerData.PassiveWeaponsList.Count > 0)
            SetPassiveWeapon(PlayerScript.playerData.PassiveWeaponsList[0].id);
        UiScript = WorldObj.GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPassiveWeapon(int id)
    {
        GameObject nouvelObjet = Instantiate(PassiveWeaponPrefab[id], PlayerObj.transform);
        nouvelObjet.transform.SetParent(PlayerObj.transform);
        nouvelObjet.transform.localPosition = new Vector3(0, 0, 1.5f);
        ItemStats scriptStats = nouvelObjet.GetComponent<ItemStats>();
        itemstats.Add(scriptStats);   
    }
    public void SetActifWeapon(ItemData weapon)
    {
        Image imageDuSlot = UiScript.HotBarSlot[PlayerScript.playerData.ActiveWeaponsList.Count - 1].GetComponent<Image>();
        imageDuSlot.sprite = PlayerScript.playerData.ActiveWeaponsList[PlayerScript.playerData.ActiveWeaponsList.Count - 1].icon;
    }
}
