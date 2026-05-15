using UnityEngine;
public class Spells : MonoBehaviour
{

    public GameObject WorldObj;
    public GameObject PlayerObj;

    private Player playerScript;
    public GameObject[] PassifWeaponPrefab;
    public GameObject[] AmmoPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = PlayerObj.GetComponent<Player>();
        if (playerScript.playerData.PassifWeaponsList.Count > 0)
            SetPassifWeapon(playerScript.playerData.PassifWeaponsList[0].id);
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
    public void SetActifWeapon(int id)
    {
        
    }
}
