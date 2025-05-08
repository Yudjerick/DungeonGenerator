using Assets.Scripts.Items;
using Mirror;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        while (Items.Count < inventorySize)
        {
            Items.Add(null);
        }
        InventoryUpdatedEvent?.Invoke();
    }
    public bool CheckCanPickUp(PickUpItem item)
    {
        if (Items[SelectedSlotIndex] == null)
        {
            return true;
        }
        var availableSlot = GetFirstAvailableSlotIndex();
        if(availableSlot != -1)
        {
            return true;
        }
        return false;
    }
    public void AddItem(InventoryItem inventoryItem, int idx)
    {
        Items[idx] = inventoryItem;
        InventoryUpdatedEvent?.Invoke();
    }
    [Server]
    public bool DropItem()
    {
        if (Items[SelectedSlotIndex] != null && Items[SelectedSlotIndex] is DroppableInventoryItem)
        {
            var instance = Instantiate(((DroppableInventoryItem)Items[SelectedSlotIndex]).PickUpItemRef);
            instance.transform.position = Items[SelectedSlotIndex].transform.position;
            instance.transform.rotation = Items[SelectedSlotIndex].transform.rotation; 
            NetworkServer.Spawn(instance.gameObject);
            RpcDropItem();
            return true;
        }
        return false;
    }
    [ClientRpc]
    public void RpcDropItem()
    {
        ((DroppableInventoryItem)Items[SelectedSlotIndex]).OnDrop();
        Items[SelectedSlotIndex] = null;
        InventoryUpdatedEvent?.Invoke();
    }
    [ClientRpc]
    public void RpcIncreaseSlotIndex()
    {
        if (_selectedSlotIndex == Items.Count - 1)
        {
            return;
        }
        _selectedSlotIndex++;
        SlotIndexUpdatedEvent?.Invoke();
    }
    [ClientRpc]
    public void RpcDecreaseSlotIndex()
    {
        if (_selectedSlotIndex == 0)
        {
            return;
        }
        _selectedSlotIndex--;
        SlotIndexUpdatedEvent?.Invoke();
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
