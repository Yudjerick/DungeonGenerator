using Mirror;
using UnityEngine;

// Rewrite
public class HandHoldable : NetworkBehaviour
{
    [SyncVar (hook = nameof(SetHandPosHook))]
    [SerializeField] private GameObject _parent;

    private Transform handPos;
    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetParentInteractionControllerObj(GameObject parent)
    {
        _parent = parent;
        handPos = _parent?.GetComponent<InteractionController>().HandPosition;
    }

    private void SetHandPosHook(GameObject oldObj, GameObject newObj)
    {
        handPos = _parent?.GetComponent<InteractionController>().HandPosition;
    }

    void Update()
    {
        if (_parent != null)
        {
            _rb.isKinematic = true;
            transform.position = handPos.transform.position;
            transform.rotation = handPos.transform.rotation;
        }
        else
        {
            _rb.isKinematic = false;

        }
        
    }
}
