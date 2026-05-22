using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Spells : MonoBehaviour
{

    public GameObject WorldObj;
    public GameObject PlayerObj;

    private Player playerScript;
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
        playerScript = PlayerObj.GetComponent<Player>();
        Transform Enfant = PlayerObj.transform.Find("ActiveWeaponManager");
        weaponManagerScript = Enfant.GetComponent<WeaponManager>();
        if (playerScript.playerData.PassiveWeaponsList.Count > 0)
            SetPassiveWeapon(playerScript.playerData.PassiveWeaponsList[0].id);
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
        itemstats.Add(nouvelObjet.GetComponent<ItemStats>());   
    }
    public void SetActifWeapon(ItemData weapon)
    {
        Image imageDuSlot = UiScript.HotBarSlot[playerScript.playerData.ActiveWeaponsList.Count - 1].GetComponent<Image>();
        imageDuSlot.sprite = playerScript.playerData.ActiveWeaponsList[playerScript.playerData.ActiveWeaponsList.Count - 1].icon;
    }
}
