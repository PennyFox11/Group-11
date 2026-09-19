using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sits on the player camera (or a child of it). Fires a ray forward every
/// frame, finds the current IInteractable (if any), shows a prompt, and
/// calls Interact() when the player presses E.
///
/// While holding an item (HeldItemController.IsHolding), E instead drops
/// it — the player's "hands are full" and the raycast stops looking for
/// new targets until they drop what they're carrying.
///
/// Input System setup:
/// - Add an "Interact" action (Button) in your Gameplay action map, bound to <Keyboard>/e.
/// - Drag the generated InputActionAsset reference in the inspector, or use
///   PlayerInput component + SendMessages/UnityEvents. This script assumes
///   PlayerInput with "Invoke Unity Events" behaviour, wired to OnInteract below.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _interactRange = 3f;
    [SerializeField] private LayerMask _interactableLayer;

    [Header("UI")]
    [SerializeField] private PromptUI _promptUI; // simple script that shows/hides a world-space or screen-space text

    private IInteractable _currentTarget;

    private void Update()
    {
        bool isHolding = HeldItemController.Instance != null && HeldItemController.Instance.IsHolding;

        if (isHolding)
        {
            // Hands are full — show a "Drop" prompt instead of scanning for new targets.
            _currentTarget = null;
            _promptUI?.Show("Drop");
            return;
        }

        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                if (interactable != _currentTarget)
                {
                    _currentTarget = interactable;
                    _promptUI?.Show(interactable.GetPrompt());
                }
                return;
            }
        }

        // Nothing hit, or hit something without IInteractable — clear.
        if (_currentTarget != null)
        {
            _currentTarget = null;
            _promptUI?.Hide();
        }
    }

    // Wire this to the "Interact" action's "performed" event via PlayerInput's
    // Unity Events, OR call it manually from an OnInteract(InputAction.CallbackContext)
    // method if you're using "Send Messages" behaviour instead. Both are shown below.
    public void OnInteractPerformed()
    {
        if (HeldItemController.Instance != null && HeldItemController.Instance.IsHolding)
        {
            HeldItemController.Instance.Drop();
            return;
        }

        _currentTarget?.Interact(this);
    }

    // Alternative if you prefer the [Send Messages] PlayerInput behaviour:
    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;

        if (HeldItemController.Instance != null && HeldItemController.Instance.IsHolding)
        {
            HeldItemController.Instance.Drop();
            return;
        }

        _currentTarget?.Interact(this);
    }
}