using Assets.Scripts.Items;
using UnityEngine;

public class InventoryEquipManager : MonoBehaviour
{
    [SerializeField] private Inventory model;

    private InventoryItem equipped;
    void Start()
    {
        equipped = null;
        model.InventoryUpdatedEvent += OnInventoryUpdate;
        model.SlotIndexUpdatedEvent += OnInventoryUpdate;
    }

    private void OnInventoryUpdate()
    {
        for(int i = 0; i < model.Items.Count; i++)
        {
            if (model.Items[i] != null)
            {
                if(model.SelectedSlotIndex == i)
                {
                    model.Items[i].UpdateShown();
                    if(equipped != model.Items[i])
                    {
                        equipped = model.Items[i];
                        model.Items[i].OnEquip();
                    }
                }
                else
                {
                    model.Items[i].UpdateHidden();
                }
            }
        }
    }
}
