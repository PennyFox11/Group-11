using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float pickupRadius = 2.5f;
    [SerializeField] private LayerMask pickupLayer;

    private PickUp currentTarget;
    private PickUp heldObject;

    private void Update()
    {
        // Don't bother scanning for new targets while already holding something
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

    // Wire this to your "Interact" (E) Input Action -> Invoke Unity Events -> Performed
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject != null)
        {
            // Already holding something -> this press drops it
            heldObject.Interact();
            heldObject = null;
        }
        else if (currentTarget != null)
        {
            // Nothing held, something nearby -> pick it up
            currentTarget.Interact();
            heldObject = currentTarget;
        }
    }

    // Wire this to your "Throw" Input Action -> Invoke Unity Events -> Performed
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
