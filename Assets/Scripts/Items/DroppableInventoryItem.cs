using Assets.Scripts.Items;
using UnityEngine;

public class DroppableInventoryItem : InventoryItem
{
    private PickUpItem _pickUp;
    public void Init(PickUpItem pickUpItem, EquipPointsProvider provider)
    {
        _pickUp = pickUpItem;
        transform.SetParent(provider.rightHand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public virtual void OnDrop()
    {
        _pickUp.gameObject.SetActive(true);
        _pickUp.transform.position = transform.position;
        _pickUp.transform.rotation = transform.rotation;
        transform.parent = _pickUp.transform;
        gameObject.SetActive(false);
    }
}
