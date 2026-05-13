using UnityEngine;

public class ChunkDecor : MonoBehaviour
{
    public GameObject treePrefab;
    public Sprite[] healthyTreeSprites;
    public Sprite[] deadTreeSprites;

    public float chunkSize = 16f;
    public float spacing = 1.2f;
    public float jitter = 0.35f;

    public float noiseScale = 0.08f;
    public float forestThreshold = 0.52f;
    public float healthyThreshold = 0.68f;

    public float scaleMin = 0.85f;
    public float scaleMax = 1.2f;

    public int worldSeed = 12345;

    public void Generate(Vector2Int chunkCoord)
    {
        int seed = worldSeed ^ (chunkCoord.x * 73856093) ^ (chunkCoord.y * 19349663);
        Random.InitState(seed);

        for (float x = 0f; x < chunkSize; x += spacing)
        {
            for (float y = 0f; y < chunkSize; y += spacing)
            {
                float jitterX = Random.Range(-jitter, jitter);
                float jitterY = Random.Range(-jitter, jitter);

                float localX = Mathf.Clamp(x + jitterX, 0f, chunkSize);
                float localY = Mathf.Clamp(y + jitterY, 0f, chunkSize);

                float worldX = transform.position.x + localX;
                float worldY = transform.position.y + localY;

                float forestNoise = Mathf.PerlinNoise(
                    (worldX + worldSeed * 0.13f) * noiseScale,
                    (worldY + worldSeed * 0.17f) * noiseScale
                );

                if (forestNoise < forestThreshold)
                    continue;

                Vector3 worldPos = new Vector3(worldX, worldY, -1f);
                GameObject tree = Instantiate(treePrefab, worldPos, Quaternion.identity, transform);

                SpriteRenderer sr = tree.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (forestNoise >= healthyThreshold && healthyTreeSprites.Length > 0)
                    {
                        sr.sprite = healthyTreeSprites[Random.Range(0, healthyTreeSprites.Length)];
                    }
                    else if (deadTreeSprites.Length > 0)
                    {
                        sr.sprite = deadTreeSprites[Random.Range(0, deadTreeSprites.Length)];
                    }
                    else if (healthyTreeSprites.Length > 0)
                    {
                        sr.sprite = healthyTreeSprites[Random.Range(0, healthyTreeSprites.Length)];
                    }

                    sr.sortingOrder = Mathf.RoundToInt(-worldY * 10f);
                }

                float scale = Random.Range(scaleMin, scaleMax);
                tree.transform.localScale = new Vector3(scale, scale, 1f);

                if (Random.value < 0.5f)
                {
                    Vector3 s = tree.transform.localScale;
                    s.x *= -1f;
                    tree.transform.localScale = s;
                }
            }
        }
    }
}