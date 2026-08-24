using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    private InteractionZone currentZone;

    public void SetCurrentZone(InteractionZone zone)
    {
        currentZone = zone;
    }

    public void ClearCurrentZone(InteractionZone zone)
    {
        // Only clear if this is the zone we're actually tracking
        // (avoids issues if two zone colliders briefly overlap)
        if (currentZone == zone)
        {
            currentZone = null;
        }
    }

    // Wire this to your "Interact" (E) Input Action -> Invoke Unity Events -> Performed
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentZone == null) return;

        currentZone.TryInteract();
    }
}