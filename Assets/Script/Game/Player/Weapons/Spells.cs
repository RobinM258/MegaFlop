using UnityEngine;
using UnityEngine.UI;

public class Spells : MonoBehaviour
{

    public GameObject WorldObj;
    public GameObject PlayerObj;

    private Player playerScript;
    private WeaponManager weaponManagerScript;
    private UIManager UiScript;
    public GameObject[] PassifWeaponPrefab;
    public GameObject[] AmmoPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = PlayerObj.GetComponent<Player>();
        Transform Enfant = PlayerObj.transform.Find("ActiveWeaponManager");
        weaponManagerScript = Enfant.GetComponent<WeaponManager>();
        if (playerScript.playerData.PassiveWeaponsList.Count > 0)
            SetPassifWeapon(playerScript.playerData.PassiveWeaponsList[0].id);
        UiScript = WorldObj.GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPassifWeapon(int id)
    {
        GameObject nouvelObjet = Instantiate(PassifWeaponPrefab[id], PlayerObj.transform);
        nouvelObjet.transform.SetParent(PlayerObj.transform);
        nouvelObjet.transform.localPosition = new Vector3(0, 0, 1.5f);    
    }
    public void SetActifWeapon(ItemData weapon)
    {
        Image imageDuSlot = UiScript.HotBarSlot[playerScript.playerData.ActiveWeaponsList.Count - 1].GetComponent<Image>();
        imageDuSlot.sprite = playerScript.playerData.ActiveWeaponsList[playerScript.playerData.ActiveWeaponsList.Count - 1].icon;
    }
}
