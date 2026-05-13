using System.Collections.Generic;
using UnityEngine;

public class ChunkBackgroundManager : MonoBehaviour
{
    public Transform player;
    public GameObject chunkPrefab;

    public int chunkSize = 16;
    public int loadRadius = 2; // 2 => charge 5x5 chunks

    private Dictionary<Vector2Int, GameObject> activeChunks = new();
    private Vector2Int currentPlayerChunk;

    void Start()
    {
        currentPlayerChunk = GetChunkCoord(player.position);
        UpdateChunks();
    }

    void Update()
    {
        Vector2Int newChunk = GetChunkCoord(player.position);

        if (newChunk != currentPlayerChunk)
        {
            currentPlayerChunk = newChunk;
            UpdateChunks();
        }
    }

    Vector2Int GetChunkCoord(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / chunkSize);
        int y = Mathf.FloorToInt(pos.y / chunkSize);
        return new Vector2Int(x, y);
    }

    void UpdateChunks()
    {
        HashSet<Vector2Int> neededChunks = new();

        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                Vector2Int coord = new Vector2Int(
                    currentPlayerChunk.x + x,
                    currentPlayerChunk.y + y
                );

                neededChunks.Add(coord);

                if (!activeChunks.ContainsKey(coord))
                {
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
                        decor.Generate(coord);
                    }

                    activeChunks.Add(coord, chunk);
                }
            }
        }

        List<Vector2Int> toRemove = new();

        foreach (var kvp in activeChunks)
        {
            if (!neededChunks.Contains(kvp.Key))
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var coord in toRemove)
        {
            activeChunks.Remove(coord);
        }
    }
}