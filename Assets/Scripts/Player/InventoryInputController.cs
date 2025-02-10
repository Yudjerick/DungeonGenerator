using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInputController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private InputAction _scrollAction;
    void Start()
    {
        _scrollAction = InputSystem.actions.FindAction("Scroll");
        _scrollAction.performed += OnScroll;
    }

    private void OnScroll(InputAction.CallbackContext context)
    {
        var scrollValue = context.ReadValue<float>();
        if(scrollValue > 0f) 
        {
            inventory.IncreaseSlotIndex();
        }
        else
        {
            inventory.DecreaseSlotIndex();
        }
    }
}
