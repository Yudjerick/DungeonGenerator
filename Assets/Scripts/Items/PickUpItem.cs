using Mirror;
using UnityEngine;
using UnityEngine.InputSystem.XR;
/// <summary>
/// Represents item's picking up functionslity. 
/// </summary>
public class PickUpItem : NetworkBehaviour, Interactable
{
    public bool IsInteractable { get => true; set => throw new System.NotImplementedException(); }
    [field: SerializeField] public DroppableInventoryItem InventoryItem { get; private set; }

    [Server]
    public void Interact(InteractionController controller)
    {
        if (controller.Inventory.PickUp(this))
        {
            RpcOnPickedUp(controller.gameObject);
        }
    }

    [ClientRpc]
    void RpcOnPickedUp(GameObject player)
    {
        InventoryItem.Init(this, player.GetComponent<EquipPointsProvider>());
        gameObject.SetActive(false);
    }

    public void OnDrop()
    {
        gameObject.SetActive(true);
        RpcOnDropped();
    }

    [ClientRpc]
    public void RpcOnDropped()
    {
        gameObject.SetActive(true);
        transform.position = InventoryItem.transform.position;
        transform.rotation = InventoryItem.transform.rotation;
        InventoryItem.transform.SetParent(transform);
        InventoryItem.gameObject.SetActive(false);
    }

    public void OnHoverEnter(InteractionController controller)
    {
    }

    public void OnHoverExit(InteractionController controller)
    {
    }
}
