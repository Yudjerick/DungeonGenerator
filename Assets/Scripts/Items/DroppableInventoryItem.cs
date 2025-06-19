using Assets.Scripts.Items;
using UnityEngine;

public class DroppableInventoryItem : InventoryItem
{
    [field: SerializeField] public PickupItem PickUpItemRef { get; private set; }

    public virtual void OnDrop()
    {
        Destroy(gameObject);
    }
}
