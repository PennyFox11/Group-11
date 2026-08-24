using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInspector : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("How close the player needs to be to an inspectable object for it to become the current target.")]
    [SerializeField] private float detectionRadius = 2.5f;
    [SerializeField] private LayerMask inspectableLayer;

    private InspectableObject currentTarget;
    private InspectableObject inspectingObject;

    private void Update()
    {
        FindClosestInspectable();
    }

    private void FindClosestInspectable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, inspectableLayer);

        InspectableObject closest = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            InspectableObject obj = hit.GetComponent<InspectableObject>();
            if (obj == null) continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = obj;
            }
        }

        currentTarget = closest;
    }

    // Wire this to your "Inspect" (Q) Input Action -> Invoke Unity Events -> Performed
    public void OnInspect(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (currentTarget == null) return;

        if (inspectingObject != null)
        {
            inspectingObject.StopInspecting();
        }

        inspectingObject = currentTarget;
        inspectingObject.Inspect();
    }

    // Visualise the detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
