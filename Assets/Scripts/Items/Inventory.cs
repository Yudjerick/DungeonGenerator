using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public List<InventoryItem> Items { get; private set; }
    [field: SerializeField] public int SelectedSlotIndex {  get; private set; } = 0;

    [SerializeField] private int inventorySize = 4;

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
            return true;
        }
        var availableSlot = GetFirstAvailableSlotIndex();
        if(availableSlot != -1)
        {
            Items[availableSlot] = item;
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
