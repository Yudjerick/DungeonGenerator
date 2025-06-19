using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents UI for single inventory slot. Incapsulates item icon setting and slot highlighting logic.
/// </summary>
public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject itemContainer;
    [SerializeField] private Image cooldownClockEffect;
    private Image _containerImage;

    private void OnEnable()
    {
        _containerImage = itemContainer.GetComponent<Image>();
    }

    public void SetIcon(Sprite icon)
    {
        itemIcon.sprite = icon;
    }

    public void SetIconTransparent(bool transparent)
    {
        if (transparent)
        {
            itemIcon.color = Color.clear;
        }
        else
        {
            itemIcon.color = Color.white;
        }
    }
    
    public void Highlight()
    {
        _containerImage.color = Color.blue;
    }

    public void UnHighlight()
    {
        _containerImage.color = Color.white;
    }

    public void UpdateCooldownClockEffect(float value)
    {
        cooldownClockEffect.fillAmount = value;
    }
}
