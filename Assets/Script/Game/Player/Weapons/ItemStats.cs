using UnityEngine;

public class ItemStats : MonoBehaviour
{
    public ItemData item;
    [SerializeField]
    private ItemData itemRef;

    public void SetUpItem()
    {
        if (item != null)
            return;
        ItemData instanceData = ScriptableObject.CreateInstance<ItemData>();
        instanceData.CopyFrom(itemRef);
        item = instanceData;
    }

    public void RefreshSize()
    {
        transform.localScale = new Vector3(item.Size, item.Size, item.Size);
    }

    void Awake()
    {
        SetUpItem();
    }

    void Update()
    {
        
    }
}
