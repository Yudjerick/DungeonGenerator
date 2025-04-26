using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private InteractionController interactionController;

    private InputAction _interactAction;
    private InputAction _scrollAction;
    private InputAction _dropAction;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _jumpAction;
    private InputAction _useAction;


    void OnEnable()
    {
        interactionController = GetComponent<InteractionController>();

        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.performed += OnInteract;
        _scrollAction = InputSystem.actions.FindAction("Scroll");
        _scrollAction.performed += OnScroll;
        _dropAction = InputSystem.actions.FindAction("Drop");
        _dropAction.performed += OnDrop;
        _useAction = InputSystem.actions.FindAction("Use");
        _useAction.performed += OnUse;

        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");

        _sprintAction = InputSystem.actions.FindAction("Sprint");
        _sprintAction.performed += OnSprintButtonStateChanged;
        _sprintAction.canceled += OnSprintButtonStateChanged;

        _jumpAction = InputSystem.actions.FindAction("Jump");
        _jumpAction.performed += OnJumpButtonPressed;
    }

    private void OnDisable()
    {
        _interactAction.performed -= OnInteract;
        _scrollAction.performed -= OnScroll;
        _dropAction.performed -= OnDrop;
        _useAction.performed -= OnUse;
        _sprintAction.performed -= OnSprintButtonStateChanged;
        _sprintAction.canceled -= OnSprintButtonStateChanged;
        _jumpAction.performed -= OnJumpButtonPressed;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interactionController.Interact();
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        interactionController.OnUse();
    }

    private void OnScroll(InputAction.CallbackContext context)
    {
        interactionController.OnScroll(context.ReadValue<float>());
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        interactionController.OnDrop();
    }

    private void OnSprintButtonStateChanged(InputAction.CallbackContext context)
    {
        playerMovement.OnSprintButtonStateChanged(context.performed);
    }

    private void OnJumpButtonPressed(InputAction.CallbackContext context)
    {
        playerMovement.OnJumpButtonPressed();
    }


    private void Update()
    {
        playerMovement.HandleMovement(_moveAction.ReadValue<Vector2>(), _lookAction.ReadValue<Vector2>());
    }
}
