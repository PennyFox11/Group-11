using UnityEngine;

/// <summary>
/// Owns the "currently held" object. Instead of destroying pickups,
/// InteractableItem hands itself off to this controller, which physically
/// parents the real object to a hold point (positioned wherever you want
/// it on screen — bottom-right for a classic FPS hold spot) and disables
/// its collider so it doesn't interfere with the world or the raycast
/// while carried.
///
/// Only one item can be held at a time. Pressing Interact while already
/// holding something drops it (see the updated PlayerInteractor) instead
/// of trying to pick up whatever's newly in view.
/// </summary>
public class HeldItemController : MonoBehaviour
{
    public static HeldItemController Instance { get; private set; }

    [Tooltip("Child of the camera, positioned where you want the held object to sit on screen (e.g. bottom-right).")]
    [SerializeField] private Transform _holdPoint;
    [Tooltip("Slow spin while held, purely cosmetic. 0 = no rotation.")]
    [SerializeField] private float _autoRotateSpeed = 20f;

    public bool IsHolding => _currentItem != null;

    private InteractableItem _currentItem;
    private Transform _originalParent;
    private Collider _currentCollider;
    private Rigidbody _currentRigidbody;
    private bool _originalKinematic;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_currentItem != null && _autoRotateSpeed != 0f)
        {
            _currentItem.transform.Rotate(Vector3.up, _autoRotateSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void PickUp(InteractableItem item)
    {
        if (_currentItem == item) return; // already holding this exact object

        if (_currentItem != null)
        {
            Drop(); // safety net — normally PlayerInteractor stops new pickups while holding, so this shouldn't trigger
        }

        _currentItem = item;
        Transform itemTransform = item.transform;

        _originalParent = itemTransform.parent;

        _currentCollider = itemTransform.GetComponent<Collider>();
        if (_currentCollider != null) _currentCollider.enabled = false;

        _currentRigidbody = itemTransform.GetComponent<Rigidbody>();
        if (_currentRigidbody != null)
        {
            _originalKinematic = _currentRigidbody.isKinematic;
            _currentRigidbody.isKinematic = true;
        }

        itemTransform.SetParent(_holdPoint);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;

        item.OnPickedUp();
    }

    public void Drop()
    {
        if (_currentItem == null) return;

        Transform itemTransform = _currentItem.transform;

        // SetParent with worldPositionStays (default true) leaves the object
        // exactly where it currently is in the world — i.e. it drops from
        // wherever your hand was, not back to where it started.
        itemTransform.SetParent(_originalParent);

        if (_currentCollider != null) _currentCollider.enabled = true;
        if (_currentRigidbody != null) _currentRigidbody.isKinematic = _originalKinematic;

        _currentItem.OnDropped();

        _currentItem = null;
        _currentCollider = null;
        _currentRigidbody = null;
    }
}