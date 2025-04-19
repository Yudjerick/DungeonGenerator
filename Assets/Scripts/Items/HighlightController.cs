using Assets.Scripts.Items;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent (typeof(InventoryItem))]
public class HighlightController : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;
    private MeshRenderer _renderer;
    private Material[] _defaultMaterials;
    private Material[] _highlightedMaterials;

    private InventoryItem item;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _defaultMaterials = _renderer.materials;
        _highlightedMaterials = new Material[2];
        _highlightedMaterials[0] = _renderer.material;
        _highlightedMaterials[1] = highlightMaterial;
        
    }

    private void OnEnable()
    {
        item = GetComponent<InventoryItem>();
        item.OnHoverEnterEvent += Highlight;
        item.OnHoverExitEvent += UnHighlight;
    }

    private void OnDisable()
    {
        item.OnHoverEnterEvent -= Highlight;
        item.OnHoverExitEvent -= UnHighlight;
    }

    private void Highlight()
    {
        _renderer.materials = _highlightedMaterials;
    }

    private void UnHighlight()
    {
        _renderer.materials = _defaultMaterials;
    }
}
