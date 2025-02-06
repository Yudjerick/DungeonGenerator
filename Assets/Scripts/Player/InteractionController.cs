using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    private InputAction _interactAction;
    [SerializeField] private float interactionDistance = 2f;
    void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.performed += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        print("e");
        var camera = Camera.main;
        var wasHit = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitInfo, interactionDistance);
        if(wasHit)
        {
            hitInfo.collider.gameObject.GetComponent<Interactable>()?.Interact(this);
        }
    }
}
