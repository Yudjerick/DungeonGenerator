using Assets.Scripts.Items;
using Mirror;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations.Rigging;
using UnityEngine;

public class NetworkInventorySync : NetworkBehaviour
{
    private Inventory _inventory;
    public readonly SyncList<GameObject> itemObjects = new SyncList<GameObject>();
    [SyncVar (hook = nameof(UpdateSlotIndexOnClientHook))] public int _selectedSlotIndex;
    void Start()
    {
        itemObjects.OnAdd += UpdateClientInventory;
        _inventory = GetComponent<Inventory>();
        if(isServer)
        {
            _inventory.InventoryUpdatedEvent += OnInventoryUpdated;
            _inventory.SlotIndexUpdatedEvent += OnSlotIndexUpdated;
        }
        
    }
    private void OnSlotIndexUpdated()
    {
        _selectedSlotIndex = _inventory.SelectedSlotIndex;
    }
    private void UpdateSlotIndexOnClientHook(int oldVal, int newVal)
    {
        if (isServer)
        {
            return;
        }
        _inventory.SelectedSlotIndex = newVal;
    }
    private void OnInventoryUpdated()
    {
        if (!isServer)
        {
            return;
        }
        var newInventoryObjects = new List<GameObject>();
        foreach (var item in _inventory.Items) 
        {
            if(item != null)
            {
                newInventoryObjects.Add(item.gameObject);
            }
            else
            {
                newInventoryObjects.Add(null);
            }
            
        }
        itemObjects.Clear();
        itemObjects.AddRange(newInventoryObjects);
    }
    private void UpdateClientInventory(int index)
    {
        if (isServer) 
        {
            return;
        }
        _inventory.Items.Clear();
        foreach(var itemObj in itemObjects)
        {
            if(itemObj != null)
            {
                _inventory.Items.Add(itemObj?.GetComponent<InventoryItem>());
            }
            else { _inventory.Items.Add(null);}
            
        }
        _inventory.InventoryUpdatedEvent?.Invoke(); 
    }
}
