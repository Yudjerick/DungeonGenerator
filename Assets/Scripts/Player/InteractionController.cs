using Assets.Scripts.Items;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private Inventory inventory;
    [field: SerializeField] public Transform HandPosition { get; private set; }
    public Inventory Inventory { get => inventory; set => inventory = value; }

    [SerializeField] private Transform cameraPos;

    private Collider _hoveredObject = null;

    private InventoryItem _equippedItem = null;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }
    void Update()
    {
        //HandleHover();
    }

    /*private void HandleHover()
    {
        
        var wasHit = Physics.Raycast(cameraPos.position, cameraPos.forward, out RaycastHit hitInfo, interactionDistance);
        if (hitInfo.collider != _hoveredObject)
        {
            _hoveredObject?.GetComponent<Interactable>()?.OnHoverExit(this);
            _hoveredObject = null;
            if (wasHit)
            {
                Interactable interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
                if (interactable != null && interactable.IsInteractable)
                {
                    hitInfo.collider.gameObject.GetComponent<Interactable>()?.OnHoverEnter(this);
                }
                
                _hoveredObject = hitInfo.collider;
            }
        }

    }*/

    [Server]
    public void Interact()
    {
        var wasHit = Physics.Raycast(cameraPos.position, cameraPos.forward, out RaycastHit hitInfo, interactionDistance);
        if (wasHit)
        {
            Interactable interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
            if (interactable != null && interactable.IsInteractable)
            {
                interactable.Interact(this);
            }
            
        }
    }

    [Server]
    public void OnScroll(float scrollValue)
    {
        if (scrollValue > 0f)
        {
            inventory.IncreaseSlotIndex();
        }
        else
        {
            inventory.DecreaseSlotIndex();
        }
    }

    [Server]
    public void OnUse()
    {

    }

    [Server]
    public void OnDrop()
    {

        inventory.DropItem();
    }

    [Server]
    public bool AddItemToInventory(InventoryItem item)
    {
        return inventory.AddItem(item);
    }

    private void OnInventoryUpdated() // think about it later
    {
    }

    private void UpdateEquippedItem()
    {

    }
}
