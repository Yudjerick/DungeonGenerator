using Assets.Scripts.Items;
using Mirror;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : NetworkBehaviour
{
    public List<InventoryItem> Items;
    public int SelectedSlotIndex
    {
        get => _selectedSlotIndex; 
        set 
        {
            if (_selectedSlotIndex < 0 || _selectedSlotIndex >= Items.Count) 
            {
                return;
            }
            _selectedSlotIndex = value;
            SlotIndexUpdatedEvent?.Invoke();
        } 
    }
    [SerializeField] private int _selectedSlotIndex;

    [SerializeField] private int inventorySize = 4;
    public Action InventoryUpdatedEvent;
    public Action SlotIndexUpdatedEvent;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Items = new List<InventoryItem>();
        for (int i = 0; i < inventorySize; i++)
        {
            Items.Add(null);
        }
    }
    public bool PickUp(PickUpItem item)
    {
        if (Items[SelectedSlotIndex] == null)
        {
            RpcAddItem(item.gameObject, SelectedSlotIndex);
            return true;
        }
        var availableSlot = GetFirstAvailableSlotIndex();
        if(availableSlot != -1)
        {
            RpcAddItem(item.gameObject, availableSlot);
            return true;
        }
        return false;
    }

    [ClientRpc]
    public void RpcAddItem(GameObject pickUpItem, int idx)
    {
        Items[idx] = pickUpItem.GetComponent<PickUpItem>().InventoryItem;
        InventoryUpdatedEvent?.Invoke();
    }

    [Server]
    public bool DropItem()
    {
        if (Items[SelectedSlotIndex] != null && Items[SelectedSlotIndex] is DroppableInventoryItem)
        {
            var pickUpItem = ((DroppableInventoryItem)Items[SelectedSlotIndex]).PickUp;
            pickUpItem.OnDrop();
            RpcDropItem();
            return true;
        }
        return false;
    }

    [ClientRpc]
    public void RpcDropItem()
    {
        var droppedItem = (DroppableInventoryItem)Items[SelectedSlotIndex];
        Items[SelectedSlotIndex] = null;
        InventoryUpdatedEvent?.Invoke();
    }

    public bool IncreaseSlotIndex()
    {
        if (_selectedSlotIndex == Items.Count - 1)
        {
            return false;
        }
        _selectedSlotIndex++;
        SlotIndexUpdatedEvent?.Invoke();
        return true;
    }

    public bool DecreaseSlotIndex()
    {
        if (_selectedSlotIndex == 0)
        {
            return false;
        }
        _selectedSlotIndex--;
        SlotIndexUpdatedEvent?.Invoke();
        return true;
    }

    public int GetItemCount()
    {
        return Items.Select(x => x != null).Count();
    }

    public int GetFirstAvailableSlotIndex()
    {
        for(int i  = 0; i < Items.Count; i++) 
        {
            if (Items[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}
