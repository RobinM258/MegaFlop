using UnityEngine;

public class ChunkBackgroundManager : MonoBehaviour
{
    public GameObject chunkPrefab;

    public int chunkSize = 16;
    public int mapWidthInChunks = 10;
    public int mapHeightInChunks = 10;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        int startX = -mapWidthInChunks / 2;
        int startY = -mapHeightInChunks / 2;

        for (int x = 0; x < mapWidthInChunks; x++)
        {
            for (int y = 0; y < mapHeightInChunks; y++)
            {
                Vector2Int coord = new Vector2Int(startX + x, startY + y);

                Vector3 worldPos = new Vector3(
                    coord.x * chunkSize,
                    coord.y * chunkSize,
                    0f
                );

                GameObject chunk = Instantiate(chunkPrefab, worldPos, Quaternion.identity, transform);
                chunk.name = $"Chunk_{coord.x}_{coord.y}";

                ChunkDecor decor = chunk.GetComponent<ChunkDecor>();
                if (decor != null)
                {
                    decor.chunkSize = chunkSize;
                    decor.Generate(coord);
                }
                ChunkGrassDecor grassDecor = chunk.GetComponent<ChunkGrassDecor>();
                if (grassDecor != null)
                {
                    grassDecor.chunkSize = chunkSize;
                    grassDecor.Generate(coord);
                }

            }
        }
    }
}