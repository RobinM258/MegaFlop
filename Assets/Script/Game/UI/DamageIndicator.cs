using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public TextMeshProUGUI textMesh; 

    private float disappearTimer = 0.5f; 
    private Color textColor;
    private float moveYSpeed = 0.8f;    

    private void Awake()
    {
       
        if (textMesh == null)
        {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void Setup(float damageAmount)
    {
        if (textMesh == null)
        {
            Debug.LogError("Le composant Texte est introuvable sur le préfab !");
            return;
        }

        textMesh.text = damageAmount.ToString("F0");
        
        textColor = textMesh.color;
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed * Time.deltaTime, 0);

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= 4f * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}