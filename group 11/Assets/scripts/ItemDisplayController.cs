using UnityEngine;

/// <summary>
/// Lives on the "Item Display Rig" — a small hidden camera setup off in
/// empty space (or on its own isolated layer) that renders the currently
/// held item to a RenderTexture. That RenderTexture is shown on a RawImage
/// on the HUD, giving the "turning the object in your hand" effect.
///
/// This does NOT touch InfoPanelUI's text — that keeps working exactly as
/// before, reading the same ItemData. This script only handles the visual
/// 3D render side. Put both on screen at once (text one side, render the
/// other) by positioning their UI elements accordingly.
/// </summary>
public class ItemDisplayController : MonoBehaviour
{
    [SerializeField] private Transform _displayAnchor; // empty child transform — spawn point for the model
    [SerializeField] private float _rotationSpeed = 30f; // degrees/sec, the "turntable" spin
    [SerializeField] private string _displayLayerName = "ItemDisplay";

    private GameObject _currentInstance;

    private void OnEnable()
    {
        GameEvents.OnItemModelRequested += ShowItem;
    }

    private void OnDisable()
    {
        GameEvents.OnItemModelRequested -= ShowItem;
    }

    public void ShowItem(GameObject prefab)
    {
        if (_currentInstance != null)
        {
            Destroy(_currentInstance);
            _currentInstance = null;
        }

        if (prefab == null) return; // this item has no 3D model — fine, text panel still shows via InfoPanelUI

        _currentInstance = Instantiate(prefab, _displayAnchor.position, Quaternion.identity, _displayAnchor);
        SetLayerRecursively(_currentInstance, LayerMask.NameToLayer(_displayLayerName));

        // Strip anything that shouldn't exist on a display copy — colliders,
        // rigidbodies, scripts meant for world behaviour. Simplest for a
        // prototype: just don't put those on the displayModel prefab at all
        // (use a clean mesh-only prefab, separate from the world pickup prefab).
    }

    public void ClearDisplay()
    {
        if (_currentInstance != null)
        {
            Destroy(_currentInstance);
            _currentInstance = null;
        }
    }

    private void Update()
    {
        if (_currentInstance != null)
        {
            _currentInstance.transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
