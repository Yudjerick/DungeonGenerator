using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandShipButton : MonoBehaviour, Interactable
{
    public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
    private bool _isInteractable = true;

    [Scene]
    public string SceneToLoad;

    public void Interact(InteractionController controller)
    {
        NetworkManager.singleton.ServerChangeScene(SceneToLoad);
    }

    public void OnHoverEnter(InteractionController controller)
    {
        
    }

    public void OnHoverExit(InteractionController controller)
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
