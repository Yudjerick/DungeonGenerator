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


        inventory.SlotIndexUpdatedEvent += UpdateEquippedItem;
        inventory.InventoryUpdatedEvent += OnInventoryUpdated;
    }
    void Update()
    {
        HandleHover();
    }

    private void HandleHover()
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

    }

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

    public void OnUse()
    {
        _equippedItem?.CmdUse();
    }

    public void OnDrop()
    {

        var dropedItem = inventory.DropItem();
        if (dropedItem != null)
        {
            _equippedItem = null;
            dropedItem.gameObject.SetActive(true);
            dropedItem.transform.position = HandPosition.position;
            dropedItem.transform.parent = null;
            dropedItem.GetComponent<Rigidbody>().isKinematic = false; //rewrite later
            dropedItem.IsInteractable = true;
            dropedItem.Player = null;
            print("Drop");
        }

    }

    public bool AddItemToInventory(InventoryItem item)
    {

        var success = inventory.AddItem(item);
        if (success)
        {
            item.IsInteractable = false;
            item.Player = gameObject;
        }
        
        return success;
    }

    private void OnInventoryUpdated() // think about it later
    {

        foreach (InventoryItem item in inventory.Items)
        {
            if (item != null)
            {
                //item.GetComponent<HandHoldable>().SetParentInteractionControllerObj(gameObject);
                item.transform.parent = HandPosition;
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.gameObject.SetActive(false);
            }

        }
        if (_equippedItem != null && inventory.Items[inventory.SelectedSlotIndex] == null)
        {
            _equippedItem.gameObject.SetActive(true);
            //_equippedItem.GetComponent<HandHoldable>().SetParentInteractionControllerObj(null);
            _equippedItem.transform.parent = null;

            _equippedItem.GetComponent<Rigidbody>().isKinematic = false;
        }
        _equippedItem = inventory.Items[inventory.SelectedSlotIndex];
        _equippedItem?.gameObject.SetActive(true);
    }

    private void UpdateEquippedItem()
    {
        print("Upd Equip");
        if (inventory.Items[inventory.SelectedSlotIndex] == _equippedItem)
        {
            return;
        }
        _equippedItem?.gameObject.SetActive(false);
        _equippedItem = inventory.Items[inventory.SelectedSlotIndex];
        _equippedItem?.gameObject.SetActive(true);


    }
}
