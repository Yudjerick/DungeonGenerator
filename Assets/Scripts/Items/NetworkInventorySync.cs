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

    void Start()
    {
        itemObjects.OnAdd += RpcUpdateClientInventory;
        _inventory = GetComponent<Inventory>();
        if(isServer)
        {
            _inventory.InventoryUpdatedEvent += OnInventoryUpdated;
        }
        
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
        //RpcUpdateClientInventory();
    }

    //[ClientRpc]
    private void RpcUpdateClientInventory(int index)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
