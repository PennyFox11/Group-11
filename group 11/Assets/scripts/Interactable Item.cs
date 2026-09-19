using UnityEngine;

/// <summary>
/// Attach to any physical object the player can pick up: the pie, the photo,
/// the prescription bottle, etc.
///
/// Picking it up hands it off to HeldItemController, which physically moves
/// the real object to the hold point (not a destroy/deactivate — you keep
/// carrying the actual thing). Its info panel shows while held and hides
/// when dropped; the clue notification only fires the first time it's
/// picked up, so re-grabbing a dropped item doesn't spam the notification.
///
/// This is also the ONE script that decides "is this a clue or not" — if
/// clueData is null, it's just flavor/info; if assigned, first pickup also
/// raises OnClueDetected.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractableItem : MonoBehaviour, IInteractable
{
    [Header("Content")]
    [Tooltip("The pre-built panel GameObject (in your Canvas, left side) that shows this specific object's info. Custom text/layout per object — not shared.")]
    [SerializeField] private GameObject _infoPanel;
    [Tooltip("Optional. A clean, mesh-only prefab for the separate 3D render rig (ItemDisplayController), if you're using that INSTEAD of physically holding this object. Usually leave empty if this object is held directly.")]
    [SerializeField] private GameObject _displayModel;
    [Tooltip("Leave empty if this object is not a clue.")]
    [SerializeField] private ClueData _clueData;

    [Header("Behaviour")]
    [SerializeField] private string _promptText = "Pick up";

    private bool _clueAlreadyFired;

    public string GetPrompt() => _promptText;

    public void Interact(PlayerInteractor player)
    {
        HeldItemController.Instance.PickUp(this);
    }

    // Called by HeldItemController once the physical hand-off is done.
    public void OnPickedUp()
    {
        if (_infoPanel != null)
        {
            GameEvents.RaiseItemPanelRequested(_infoPanel);
        }

        if (_displayModel != null)
        {
            GameEvents.RaiseItemModelRequested(_displayModel);
        }

        if (_clueData != null && !_clueAlreadyFired)
        {
            GameEvents.RaiseClueDetected(_clueData);
            _clueAlreadyFired = true;
        }
    }

    // Called by HeldItemController when the player drops this item.
    public void OnDropped()
    {
        PanelSwitcher.Instance?.HideCurrent();
    }
}