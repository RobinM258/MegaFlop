using UnityEngine;
public class Spells : MonoBehaviour
{

    public GameObject WorldObj;
    public GameObject PlayerObj;

    public GameObject[] SpellPrefab;
    public GameObject[] AmmoPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSpells(GameData.SelectedCharacterIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpells(int id)
    {
        GameObject nouvelObjet = Instantiate(SpellPrefab[id], PlayerObj.transform);
        nouvelObjet.transform.SetParent(PlayerObj.transform);
        nouvelObjet.transform.localPosition = new Vector3(0, 0, 1.5f);       
    }
}
