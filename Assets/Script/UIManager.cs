using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{

    [Header("Player Stats")]
    public GameObject[] Pannels;
    public TMP_Text FPSCount;

    private bool isPaused = false;
    private float deltaTime = 0;
    private float deltaTimeFPS = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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

    public void OnCancel(InputValue value)
    {
        isPaused = !isPaused;
        
        Pannels[0].SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }
}
