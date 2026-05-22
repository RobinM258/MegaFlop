using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public ItemData item;
    public ItemData itemRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ItemData instanceData = ScriptableObject.CreateInstance<ItemData>();
        instanceData.CopyFrom(itemRef);
        item = instanceData;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
