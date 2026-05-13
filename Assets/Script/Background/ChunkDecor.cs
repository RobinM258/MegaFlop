using UnityEngine;

public class ChunkDecor : MonoBehaviour
{
    public GameObject treePrefab;

    public Sprite[] healthyTreeSprites;
    public Sprite[] deadTreeSprites;

    [Header("Chunk")]
    public float chunkSize = 16f;
    public int worldSeed = 12345;

    [Header("Placement")]
    public float spacing = 1.3f;
    public float jitter = 0.3f;

    [Header("Forest Shape")]
    public float forestNoiseScale = 0.07f;
    public float detailNoiseScale = 0.18f;
    public float forestThreshold = 0.54f;
    public float healthyThreshold = 0.72f;

    [Header("Visual")]
    public float scaleMin = 0.75f;
    public float scaleMax = 1.25f;
    public string sortingLayerName = "Objects";

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

                float largeForest = Mathf.PerlinNoise(
                    (worldX + worldSeed * 0.13f) * forestNoiseScale,
                    (worldY + worldSeed * 0.17f) * forestNoiseScale
                );

                float detail = Mathf.PerlinNoise(
                    (worldX + worldSeed * 0.31f) * detailNoiseScale,
                    (worldY + worldSeed * 0.29f) * detailNoiseScale
                );

                float combinedNoise = largeForest * 0.75f + detail * 0.25f;

                if (combinedNoise < forestThreshold)
                    continue;

                Vector3 worldPos = new Vector3(worldX, worldY, -1f);
                GameObject tree = Instantiate(treePrefab, worldPos, Quaternion.identity, transform);

                SpriteRenderer sr = tree.GetComponent<SpriteRenderer>();
                if (sr == null)
                    sr = tree.GetComponentInChildren<SpriteRenderer>();

                if (sr != null)
                {
                    sr.sortingLayerName = sortingLayerName;
                    sr.sortingOrder = Mathf.RoundToInt(-worldY * 100f);

                    if (combinedNoise >= healthyThreshold && healthyTreeSprites.Length > 0)
                    {
                        int index = Random.Range(0, healthyTreeSprites.Length);
                        sr.sprite = healthyTreeSprites[index];
                    }
                    else if (deadTreeSprites.Length > 0)
                    {
                        int index = Random.Range(0, deadTreeSprites.Length);
                        sr.sprite = deadTreeSprites[index];
                    }
                    else if (healthyTreeSprites.Length > 0)
                    {
                        int index = Random.Range(0, healthyTreeSprites.Length);
                        sr.sprite = healthyTreeSprites[index];
                    }
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