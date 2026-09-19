using UnityEngine;
using UnityEngine.InputSystem;
//Akhona Khoali
//This script is used to every time a player enters an interation Zone, it works similarily to an on trigger method but it is more specific to the interaction system  and allows for differnt interactions with the same button press(E)

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
        // stops two zones from being activated at the same time and causes a bug. Side note I noticed a bug when unlocking the safe door because the pickup object is in the safe and the safe itself required the E input so afterunlockingthe same the object automatically clips to the player as a pick up. I dont know how to fix it for now but I will try to fix it later.
        if (currentZone == zone)
        {
            currentZone = null;
        }
    }

    // Represents that it should be added to the event system in the input manager and called when the player presses E
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentZone == null) return;

        currentZone.TryInteract();
    }
}