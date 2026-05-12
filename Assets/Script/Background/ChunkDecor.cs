using UnityEngine;

public class ChunkDecor : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public int minTrees = 0;
    public int maxTrees = 30;
    public float chunkMinSize = 1f;
    public float chunkMaxSize = 16f;
    public int worldSeed = 12345;

    public void Generate(Vector2Int chunkCoord)
    {
        int seed = worldSeed ^ (chunkCoord.x * 73856093) ^ (chunkCoord.y * 19349663);
        Random.InitState(seed);

        int treeCount = Random.Range(minTrees, maxTrees + 1);
        float chunkSize = Random.Range(chunkMinSize, chunkMaxSize);

        for (int i = 0; i < treeCount; i++)
        {
            float x = Random.Range(0f, chunkSize);
            float y = Random.Range(0f, chunkSize);

            Vector3 localPos = new Vector3(x, y, 0f);
            int treeIndex = Random.Range(0, treePrefabs.Length);

            Instantiate(treePrefabs[treeIndex], transform.position + localPos, Quaternion.identity, transform);
        }
    }
}