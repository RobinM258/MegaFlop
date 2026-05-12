using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundChunk : MonoBehaviour
{
    public Vector2 size = new Vector2(16, 16);

    void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = size;
    }
}