using Assets.Scripts.Items;
using Mirror;
using UnityEngine;

public class InteractionController : NetworkBehaviour
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
    [Command]
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
    [Command]
    public void OnScroll(float scrollValue)
    {
        if (scrollValue > 0f)
        {
            inventory.RpcIncreaseSlotIndex();
        }
        else
        {
            inventory.RpcDecreaseSlotIndex();
        }
    }
    [Command]
    public void OnUse()
    {

    }
    [Command]
    public void OnDrop()
    {

        inventory.DropItem();
    }
}
