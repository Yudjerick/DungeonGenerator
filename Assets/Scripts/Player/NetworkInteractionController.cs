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
    // Update is called once per frame
    void Update()
    {
        
    }
}
