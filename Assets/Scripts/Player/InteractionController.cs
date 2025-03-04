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
        if(hitInfo.collider != _hoveredObject)
        {
            _hoveredObject?.GetComponent<Interactable>()?.OnHoverExit(this);
            _hoveredObject = null;
            if (wasHit)
            {
                hitInfo.collider.gameObject.GetComponent<Interactable>()?.OnHoverEnter(this);
                _hoveredObject = hitInfo.collider;
            }
        }
        
    }

    public void Interact()
    {
        var wasHit = Physics.Raycast(cameraPos.position, cameraPos.forward, out RaycastHit hitInfo, interactionDistance);
        if (wasHit)
        {
            hitInfo.collider.gameObject.GetComponent<Interactable>()?.Interact(this);
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
        }

    }

    public bool AddItemToInventory(InventoryItem item)
    {

        var success = inventory.AddItem(item);
        return success;
    }

    private void OnInventoryUpdated() // think about it later
    {
        
        foreach(InventoryItem item in inventory.Items) 
        {
            if(item != null)
            {
                //item.GetComponent<HandHoldable>().SetParentInteractionControllerObj(gameObject);
                item.transform.parent = HandPosition;
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.localPosition = Vector3.zero;
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
