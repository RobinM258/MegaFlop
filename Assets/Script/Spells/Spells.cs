using UnityEngine;

public class Spells : MonoBehaviour
{

    public GameObject WorldObj;
    public GameObject PlayerObj;

    public GameObject[] SpellPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameData.SelectedCharacterIndex == 0)
            SetSpells(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpells(int id)
    {
        if (id == 0)
            SetAura();
        else if (id == 1)
            SetFireBall();            

    }
    void SetAura()
    {
        GameObject nouvelObjet = Instantiate(SpellPrefab[0], PlayerObj.transform);
        nouvelObjet.transform.SetParent(PlayerObj.transform);
        nouvelObjet.transform.localPosition = new Vector3(0, 0, 1.5f);
    }

    void SetFireBall()
    {
        GameObject nouvelObjet = Instantiate(SpellPrefab[1], PlayerObj.transform);
        nouvelObjet.transform.SetParent(PlayerObj.transform);
        nouvelObjet.transform.localPosition = new Vector3(0, 0, 1.5f);
    }
}
