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

    public void SetLevelUpBTn()
    {
        for (int i = 0; i < LevelUpBtn.Length; i++)
        {
            int rdm = Random.Range(0, WorldScript.itemRemains.Length);
            Debug.Log(rdm);
            tempoItem[i] = WorldScript.item[rdm];
            LevelUpBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = WorldScript.item[rdm].itemName;
        }
    }

    public void LevelUpBtnId(int id)
    {
        Debug.Log(id);
        ClosePannel(1, false);
    }
}
