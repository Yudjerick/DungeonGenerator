using Assets.Scripts.Items;
using UnityEngine;

public class LateJoinPickupCommand
{
    public InventoryItem Item {  get; private set; }
    public Inventory Inventory { get; private set; }

    public LateJoinPickupCommand(InventoryItem item, Inventory inventory)
    {
        Item = item;
        Inventory = inventory;
    }
    public void Execute()
    {
        Inventory.AddItem(Item, false);
    }
}
