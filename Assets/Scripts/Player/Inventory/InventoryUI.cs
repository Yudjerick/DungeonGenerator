using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory model;
    [SerializeField] private List<InventorySlot> slots;
    [SerializeField] private Sprite emptySlotSprite;
    [SerializeField] private InventorySlot slotPrefab;
    private void OnEnable()
    {
        model.InventoryUpdatedEvent += UpdateUI;
        model.SlotIndexUpdatedEvent += UpdateUI;
        model.ItemStateUpdatedEvent += UpdateItemState;
    }

    private void OnDisable()
    {
        model.InventoryUpdatedEvent -= UpdateUI;
        model.SlotIndexUpdatedEvent -= UpdateUI;
        model.ItemStateUpdatedEvent -= UpdateItemState;
    }

    private void RecreateItemSlots()
    {
        foreach(InventorySlot slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
        for (int i = 0; i < model.Items.Count; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, transform);
            slots.Add(slot);
        }
    }

    private void UpdateUI()
    {
        if(slots.Count != model.Items.Count)
        {
            RecreateItemSlots();
        }
        for (int i = 0; i < slots.Count; i++)
        {
            if (model.Items[i] != null)
            {
                slots[i].SetIcon(model.Items[i].InventoryPicture);
            }
            else
            {
                slots[i].SetIcon(emptySlotSprite);
                slots[i].UpdateCooldownClockEffect(0);
            }
            if(i == model.SelectedSlotIndex)
            {
                slots[i].Highlight();
            }
            else
            {
                slots[i].UnHighlight();
            }
            
        }
    }

    private void UpdateItemState(int idx)
    {
        slots[idx].UpdateCooldownClockEffect(model.Items[idx].CooldownClockState);
    }
}
