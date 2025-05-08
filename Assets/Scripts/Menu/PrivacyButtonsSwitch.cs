using UnityEngine;
using UnityEngine.UI;

public class PrivacyButtonsSwitch : MonoBehaviour
{
    [SerializeField] private Image privateButton;
    [SerializeField] private Image publicPutton;
    [SerializeField] private SteamLobbyCreateManager manager;

    public void Switch()
    {
        if (manager.IsPublic)
        {
            HighlightButton(publicPutton);
            UnHighLightButton(privateButton);
        }
        else
        {
            HighlightButton(privateButton);
            UnHighLightButton(publicPutton);
        }
    }

    private void HighlightButton(Image image)
    {
        image.color = Color.white;
    }

    private void UnHighLightButton(Image image)
    {
        image.color = Color.gray;
    }
}
