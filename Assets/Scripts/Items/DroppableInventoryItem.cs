using Assets.Scripts.Items;
using UnityEngine;

public class DroppableInventoryItem : InventoryItem
{
    [field: SerializeField] public PickUpItem PickUpItemRef { get; private set; }
    [SerializeField] private Transform grabPoint;
    public virtual void Init(EquipPointsProvider provider)
    {
        grabPoint.transform.parent = null;
        transform.SetParent(grabPoint);
        grabPoint.transform.parent = provider.rightHand;
        grabPoint.localPosition = Vector3.zero;
        grabPoint.localRotation = Quaternion.identity;
        
    }


    public virtual void OnDrop()
    {
        Destroy(gameObject);
    }
}
