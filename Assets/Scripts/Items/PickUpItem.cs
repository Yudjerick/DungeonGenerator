using Mirror;
using UnityEngine;
/// <summary>
/// Represents item's picking up functionslity. 
/// </summary>
public class PickUpItem : NetworkBehaviour, Interactable
{
    public bool IsInteractable { get => true; set => throw new System.NotImplementedException(); }
    [SerializeField] private DroppableInventoryItem inventoryItem;

    public void Interact(InteractionController controller)
    {
        if (controller.AddItemToInventory(inventoryItem))
        {
            inventoryItem.Init(this, controller.gameObject.GetComponent<EquipPointsProvider>());
            gameObject.SetActive(false);
        }
    }

    public void OnHoverEnter(InteractionController controller)
    {
    }

    public void OnHoverExit(InteractionController controller)
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
