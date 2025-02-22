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
