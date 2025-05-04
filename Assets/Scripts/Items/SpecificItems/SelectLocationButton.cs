using Mirror;
using UnityEngine;

public class SelectLocationButton : MonoBehaviour, Interactable
{
    [SerializeField] private LandShipButton landShipButton;
    [Scene]
    [SerializeField] private string associatedScene;

    public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
    private bool _isInteractable = true;

    public void Interact(InteractionController controller)
    {
        landShipButton.SceneToLoad = associatedScene;
    }

    public void OnHoverEnter(InteractionController controller)
    {

    }

    public void OnHoverExit(InteractionController controller)
    {

    }
}
