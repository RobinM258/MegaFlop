using UnityEngine;

public class Spells : MonoBehaviour
{
    public GameObject WorldObj;
    public GameObject PlayerObj;

    private World WorldScript;
    private Player PlayerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WorldScript = WorldObj.GetComponent<World>();
        PlayerScript = PlayerObj.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeSize(float nb)
    {
        transform.localScale = new Vector3(nb, nb, nb);
    }
}
