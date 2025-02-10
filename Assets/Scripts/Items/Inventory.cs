using Assets.Scripts.Items;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public List<InventoryItem> Items { get; private set; }
    public int SelectedSlotIndex
    {
        get => _selectedSlotIndex; 
        set 
        {
            value = _selectedSlotIndex;
            InventoryUpdatedEvent?.Invoke();
        } 
    }
    [SerializeField] private int _selectedSlotIndex;

    [SerializeField] private int inventorySize = 4;
    public Action InventoryUpdatedEvent;

    private void Start()
    {
        Items = new List<InventoryItem>();
        for(int i = 0; i < inventorySize; i++)
        {
            Items.Add(null); 
        }
    }
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

    [Button]
    public bool DropItem()
    {
        if (Items[SelectedSlotIndex] != null)
        {
            var dropedItem = Items[SelectedSlotIndex];
            Items[SelectedSlotIndex] = null;
            dropedItem.gameObject.SetActive(true);
            dropedItem.transform.localPosition = Vector3.zero;
            dropedItem.transform.parent = null;
            
            InventoryUpdatedEvent?.Invoke();
            return true;
        }
        return false;
        
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
