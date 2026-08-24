using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractionZone : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [Tooltip("If true, this zone can only ever be interacted with once.")]
    [SerializeField] private bool interactOnce = false;

    [Header("Events")]
    public UnityEvent onInteract;
    public UnityEvent onZoneExit;

    private bool hasInteracted = false;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerInteractor interactor = other.GetComponent<PlayerInteractor>();
        if (interactor != null)
        {
            interactor.SetCurrentZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        PlayerInteractor interactor = other.GetComponent<PlayerInteractor>();
        if (interactor != null)
        {
            interactor.ClearCurrentZone(this);
        }

        onZoneExit?.Invoke();
    }

    public void TryInteract()
    {
        if (interactOnce && hasInteracted) return;
        hasInteracted = true;
        onInteract?.Invoke();
    }
}