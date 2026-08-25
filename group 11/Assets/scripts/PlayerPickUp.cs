using UnityEngine;
using UnityEngine.InputSystem;
//Akhona Khoali
//this is the new updated version of the player pickup that works, not the one given in class that was not working.

public class PlayerPickup : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float pickupRadius = 2.5f;
    [SerializeField] private LayerMask pickupLayer;

    private PickUp currentTarget;
    private PickUp heldObject;

    private void Update()
    {
        // if the player is holding an object already, there is no need to pick up another one, so we only check for nearby pickups when the player is not holding anything
        if (heldObject == null)
        {
            FindClosestPickup();
        }
    }

    private void FindClosestPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius, pickupLayer);

        PickUp closest = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            PickUp p = hit.GetComponent<PickUp>();
            if (p == null) continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = p;
            }
        }

        currentTarget = closest;
    }

    // this is for the input system where you add an event to the interact action and link it to the function
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject != null)
        {
            //  if the player is Already holding something,this press drops it
            heldObject.Interact();
            heldObject = null;
        }
        else if (currentTarget != null)
        {
            // if Nothing is held but something is nearby, the player can pick it up
            currentTarget.Interact();
            heldObject = currentTarget;
        }
    }

    // this is for the throw input action where you add the throw event
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObject == null) return;

        heldObject.ThrowObject();
        heldObject = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
