using UnityEngine;

public interface Interactable
{
    bool Interact(InteractionController controller);
    void OnHoverEnter(InteractionController controller);
    void OnHoverExit(InteractionController controller);
}
