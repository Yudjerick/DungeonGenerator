using Assets.Scripts.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform handPosition;

    private Collider _hoveredObject = null;
    private InputAction _interactAction;
    private InputAction _scrollAction;
    private InputAction _dropAction;

    private InventoryItem _equippedItem = null;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.performed += OnInteract;

        _scrollAction = InputSystem.actions.FindAction("Scroll");
        _scrollAction.performed += OnScroll;

        _dropAction = InputSystem.actions.FindAction("Drop");
        _dropAction.performed += OnDrop;

        inventory.SlotIndexUpdatedEvent += UpdateEquippedItem;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHover();
    }

    private void HandleHover()
    {
        var camera = Camera.main;
        var wasHit = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitInfo, interactionDistance);
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

    private void OnInteract(InputAction.CallbackContext context)
    {
        var camera = Camera.main;
        var wasHit = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitInfo, interactionDistance);
        if(wasHit)
        {
            hitInfo.collider.gameObject.GetComponent<Interactable>()?.Interact(this);
        }
    }

    private void OnScroll(InputAction.CallbackContext context)
    {
        var scrollValue = context.ReadValue<float>();
        if (scrollValue > 0f)
        {
            inventory.IncreaseSlotIndex();
        }
        else
        {
            inventory.DecreaseSlotIndex();
        }
    }

    private void OnDrop(InputAction.CallbackContext context)
    {

        var dropedItem = inventory.DropItem();
        if (dropedItem != null)
        {
            _equippedItem = null;
            dropedItem.gameObject.SetActive(true);
            dropedItem.transform.position = handPosition.position;
            dropedItem.transform.parent = null;
            dropedItem.GetComponent<Rigidbody>().isKinematic = false; //rewrite later
        }

    }

    public bool AddItemToInventory(InventoryItem item)
    {
        var success = inventory.AddItem(item);
        if (success)
        {
            item.OnHoverExit(this);
            item.gameObject.SetActive(false);
            item.transform.parent = handPosition;
            item.transform.localPosition = Vector3.zero;
            item.GetComponent<Rigidbody>().isKinematic = true;
            UpdateEquippedItem();
        }
        return success;
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
