using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    private InputAction _interactAction;
    [SerializeField] private float interactionDistance = 2f;
    private Collider _hoveredObject = null;

    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.performed += OnInteract;
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
}
