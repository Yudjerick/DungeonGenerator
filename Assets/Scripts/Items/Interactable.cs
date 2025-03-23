using UnityEngine;

public interface Interactable
{
    public bool IsInteractable { get; set; }
    bool Interact(InteractionController controller);
    void OnHoverEnter(InteractionController controller);
    void OnHoverExit(InteractionController controller);
}
