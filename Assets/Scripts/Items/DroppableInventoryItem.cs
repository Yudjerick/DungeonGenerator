using Assets.Scripts.Items;
using UnityEngine;

public class DroppableInventoryItem : InventoryItem
{
    [field: SerializeField] public PickUpItem PickUpItemRef { get; private set; }

    public virtual void OnDrop()
    {
        Destroy(gameObject);
    }
}
