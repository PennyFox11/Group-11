using UnityEngine;

/// <summary>
/// Each pickupable object references its OWN pre-built panel GameObject
/// (different text/layout per object — a call log panel looks nothing like
/// a photograph panel). This script just makes sure only one shows at a
/// time: when a new panel is requested, the previous one hides.
///
/// Setup: put every item's info panel as a child of the same parent on your
/// left-side HUD area, all inactive by default. Put this script on that
/// parent (or anywhere persistent). No per-panel wiring needed here — each
/// InteractableItem just points at its own panel.
/// </summary>
public class PanelSwitcher : MonoBehaviour
{
    public static PanelSwitcher Instance { get; private set; }

    private GameObject _currentPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnItemPanelRequested += HandlePanelRequested;
    }

    private void OnDisable()
    {
        GameEvents.OnItemPanelRequested -= HandlePanelRequested;
    }

    private void HandlePanelRequested(GameObject panel)
    {
        if (_currentPanel != null && _currentPanel != panel)
        {
            _currentPanel.SetActive(false);
        }

        if (panel != null)
        {
            panel.SetActive(true);
            _currentPanel = panel;
        }
    }

    // Call this if you want a manual "close panel" button/key somewhere.
    public void HideCurrent()
    {
        if (_currentPanel != null)
        {
            _currentPanel.SetActive(false);
            _currentPanel = null;
        }
    }
}