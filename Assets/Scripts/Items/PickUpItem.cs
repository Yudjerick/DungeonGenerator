using Mirror;
using UnityEngine;
using UnityEngine.InputSystem.XR;
/// <summary>
/// Represents item's picking up functionslity. 
/// </summary>
public class PickUpItem : NetworkBehaviour, Interactable
{
    public bool IsInteractable { get => true; set => throw new System.NotImplementedException(); }
    [field: SerializeField] public DroppableInventoryItem InventoryItemRef { get; private set; }

    private InteractionController interactionController;

    [Server]
    public void Interact(InteractionController controller)
    {
        if (controller.Inventory.PickUp(this))
        {
            interactionController = controller;
            RpcOnPickedUp(controller.gameObject);
            
            
            
        }
    }

   

    [ClientRpc]
    void RpcOnPickedUp(GameObject player)
    {
        var instance = Instantiate(InventoryItemRef);
        interactionController.Inventory.AddItem(instance, interactionController.Inventory.SelectedSlotIndex);
        instance.Init(player.GetComponent<EquipPointsProvider>());
        NetworkServer.Destroy(gameObject);
    }

    public void OnHoverEnter(InteractionController controller)
    {
    }

    public void OnHoverExit(InteractionController controller)
    {
    }
}
