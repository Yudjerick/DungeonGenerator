using Assets.Scripts.Items;
using UnityEngine;

public class DroppableInventoryItem : InventoryItem
{
    public PickUpItem PickUp { get; private set; }
    public void Init(PickUpItem pickUpItem, EquipPointsProvider provider)
    {
        PickUp = pickUpItem;
        transform.SetParent(provider.rightHand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
