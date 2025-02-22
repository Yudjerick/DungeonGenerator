using Mirror;
using UnityEngine;

public class NetworkInteractionController : NetworkBehaviour
{
    [SerializeField] InteractionController controller;
    void Start()
    {
        
    }

    public void Interact()
    {
        CmdInteract();
    }

    public void OnScroll(float scrollValue)
    {
        CmdScroll(scrollValue);
    }

    public void OnDrop()
    {
        CmdDrop();
    }

    [Command]
    private void CmdScroll(float scrollValue)
    {
        controller.OnScroll(scrollValue);
    }

    [Command] 
    public void CmdInteract()
    {
        controller.Interact();
    }

    [Command]
    public void CmdDrop()
    {
        controller.OnDrop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
