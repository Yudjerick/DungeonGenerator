using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory model;
    [SerializeField] private List<Image> slotImages;
    [SerializeField] Sprite emptySlotSprite;
    

    private void OnEnable()
    {
        model.InventoryUpdatedEvent += UpdateUI;
    }

    private void OnDisable()
    {
        model.InventoryUpdatedEvent -= UpdateUI;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (model.Items[i] != null)
            {
                slotImages[i].sprite = model.Items[i].InventoryPicture;
            }
            else
            {
                slotImages[i].sprite = emptySlotSprite;
            }
            if(i == model.SelectedSlotIndex)
            {
                slotImages[i].transform.parent.GetComponent<Image>().color = Color.blue;
            }
            else
            {
                slotImages[i].transform.parent.GetComponent<Image>().color = Color.white;
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
