using Assets.Scripts.Items;
using Mirror;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
/// <summary>
/// Represents item's picking up functionslity. 
/// </summary>
public class PickupItem : NetworkBehaviour, Interactable
{
    public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
    private bool _isInteractable = true;
    [field: SerializeField] public DroppableInventoryItem InventoryItemRef { get; private set; }

    private InteractionController interactionController;

    [SerializeField] private PickupState state;
 
    [Server]
    public void Interact(InteractionController controller)
    {
        var idx = controller.Inventory.GetFirstAvailableSlotIndex();
        if (idx != -1)
        {
            RpcOnPickedUp(controller.gameObject, GetState(), idx);
        }
    }

    [Server]
    public void SetState(StateBundle bundle)
    {
        state.SetStateFromBundle(bundle);
    }

    [Server]
    public StateBundle GetState()
    {
        return state?.GetStateBundle();
    }
   

    [ClientRpc]
    void RpcOnPickedUp(GameObject player, StateBundle stateBundle, int idx)
    {
        var instance = Instantiate(InventoryItemRef);
        interactionController = player.GetComponent<InteractionController>();
        interactionController.Inventory.AddItem(instance, idx);
        instance.Init(player.GetComponent<EquipPointsProvider>(), stateBundle);
        NetworkServer.Destroy(gameObject);
    }

    public void OnHoverEnter(InteractionController controller)
    {
    }

    public void OnHoverExit(InteractionController controller)
    {
    }
}
