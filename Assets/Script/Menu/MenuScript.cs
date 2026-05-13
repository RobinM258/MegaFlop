using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject[] Pannels;
    public GameObject PreviousPannel;
    public GameObject CurrentPannel;

    private bool LevelSelected;
    private bool PlayerSelected;


    public ItemData[] Spell;
    public ItemData CurrentItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentPannel = Pannels[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseAllPannel()
    {
        for (int i = 0; i < Pannels.Length; i++)
        {
            Pannels[i].SetActive(false);
        }
    }
    public void PlayBtn()
    {
        CloseAllPannel();
        Pannels[1].SetActive(true);
        PreviousPannel = CurrentPannel;
        CurrentPannel = Pannels[1];
    }

    public void SettingBtn()
    {
        CloseAllPannel();
        Pannels[2].SetActive(true);
        PreviousPannel = CurrentPannel;
        CurrentPannel = Pannels[2];
    }

    public void StartGame()
    {
        if (LevelSelected && PlayerSelected)
        {
            GameData.FirstItem = CurrentItem;
            SceneManager.LoadScene(GameData.LevelId);
        }
    }

    public void BackBtn()
    {
        CloseAllPannel();
        PreviousPannel.SetActive(true);
        CurrentPannel = PreviousPannel;
        PreviousPannel = null;
        LevelSelected = false;
        PlayerSelected = false;
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    public void Selectlevel(int id)
    {
        GameData.LevelId = "Level" + id;
        LevelSelected = true;
    }

    public void SelectPlayer(int id)
    {
        GameData.SelectedCharacterIndex = id;
        CurrentItem = Spell[id];
        PlayerSelected = true;
    }
}
