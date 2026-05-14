using UnityEngine;

public class ChunkGrassDecor : MonoBehaviour
{
    public GameObject grassPrefab;
    public Sprite[] grassSprites;

    [Header("Chunk")]
    public float chunkSize = 16f;
    public int worldSeed = 12345;

    [Header("Placement")]
    public float spacing = 1.1f;
    public float jitter = 0.35f;
    [Range(0f, 1f)] public float grassSpawnChance = 0.18f;

    [Header("Forest Mask")]
    public float forestNoiseScale = 0.07f;
    public float detailNoiseScale = 0.18f;
    public float forestThreshold = 0.54f;

    [Header("Grass Zone")]
    public float grassMinNoise = 0.20f;
    public float grassMaxNoise = 0.52f;

    [Header("Visual")]
    public float scaleMin = 0.8f;
    public float scaleMax = 1.15f;
    public string sortingLayerName = "Objects";

    public void Generate(Vector2Int chunkCoord)
    {
        int seed = (worldSeed + 9999) ^ (chunkCoord.x * 73856093) ^ (chunkCoord.y * 19349663);
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

                // Pas d'herbe dans la forêt dense
                if (combinedNoise >= forestThreshold)
                    continue;

                // L'herbe vit surtout hors forêt, mais pas partout pareil
                if (combinedNoise < grassMinNoise || combinedNoise > grassMaxNoise)
                    continue;

                // Placement sporadique
                if (Random.value > grassSpawnChance)
                    continue;

                Vector3 worldPos = new Vector3(worldX, worldY, -0.5f);
                GameObject grass = Instantiate(grassPrefab, worldPos, Quaternion.identity, transform);

                SpriteRenderer sr = grass.GetComponent<SpriteRenderer>();
                if (sr == null)
                    sr = grass.GetComponentInChildren<SpriteRenderer>();

                if (sr != null)
                {
                    sr.sortingLayerName = sortingLayerName;
                    sr.sortingOrder = Mathf.RoundToInt(-worldY * 100f) - 1;

                    if (grassSprites.Length > 0)
                    {
                        int index = Random.Range(0, grassSprites.Length);
                        sr.sprite = grassSprites[index];
                    }
                }

                float scale = Random.Range(scaleMin, scaleMax);
                grass.transform.localScale = new Vector3(scale, scale, 1f);

                if (Random.value < 0.5f)
                {
                    Vector3 s = grass.transform.localScale;
                    s.x *= -1f;
                    grass.transform.localScale = s;
                }
            }
        }
    }
}