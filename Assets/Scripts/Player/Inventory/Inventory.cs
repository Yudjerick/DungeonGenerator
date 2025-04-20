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

    [Server]
    public bool AddItem(InventoryItem item)
    {
        if (Items[SelectedSlotIndex] == null)
        {
            Items[SelectedSlotIndex] = item;
            InventoryUpdatedEvent?.Invoke();
            return true;
        }
        var availableSlot = GetFirstAvailableSlotIndex();
        if(availableSlot != -1)
        {
            Items[availableSlot] = item;
            InventoryUpdatedEvent?.Invoke();
            return true;
        }
        return false;
    }

    [Server]
    public bool DropItem()
    {
        if (Items[SelectedSlotIndex] != null && Items[SelectedSlotIndex] is DroppableInventoryItem)
        {
            var droppedItem = (DroppableInventoryItem)Items[SelectedSlotIndex];
            droppedItem.OnDrop();
            Items[SelectedSlotIndex] = null;
            InventoryUpdatedEvent?.Invoke();
            return true;
        }
        return false;
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
