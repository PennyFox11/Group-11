using UnityEngine;

/// <summary>
/// Attach to objects that start invisible/inert (e.g. the fingerprint on the
/// pie) and only become interactable while — and after — they're caught in
/// the scanner's beam.
///
/// Matches ScannerController's toggle-light model: this script polls
/// scanner.IsPointInBeam() each frame rather than waiting for the scanner
/// to raycast into it. Once revealed, it switches itself to the
/// "Interactable" layer and enables its visual + collider so the normal
/// E-interact flow (InteractableItem) picks it up from there.
/// </summary>
public class HiddenClue : MonoBehaviour
{
    [SerializeField] private ScannerController _scanner; // assign the player's scanner in the inspector
    [SerializeField] private GameObject _visual;          // disabled until revealed
    [SerializeField] private Collider _interactCollider;  // disabled until revealed
    [SerializeField] private string _interactableLayerName = "Interactable";
    [Tooltip("Once revealed, stays revealed even if the beam moves away. Uncheck for 'only visible while lit up'.")]
    [SerializeField] private bool _stayRevealedOnceFound = true;

    private bool _isRevealed;

    private void Awake()
    {
        if (_scanner == null)
        {
            // Fallback so you don't have to manually drag the scanner onto
            // every single hidden clue in the scene. Assumes one scanner
            // (the player's) exists. Cache it once here rather than calling
            // FindObjectOfType in Update.
#if UNITY_2023_1_OR_NEWER
            _scanner = FindFirstObjectByType<ScannerController>();
#else
            _scanner = FindObjectOfType<ScannerController>();
#endif
            if (_scanner == null)
            {
                Debug.LogWarning($"{name}: no ScannerController found in scene and none assigned — this clue can never be revealed.", this);
            }
        }

        SetRevealedVisuals(false);
    }

    private void Update()
    {
        if (_isRevealed && _stayRevealedOnceFound) return;
        if (_scanner == null) return;

        bool inBeam = _scanner.IsPointInBeam(transform.position);

        if (inBeam && !_isRevealed)
        {
            Reveal();
        }
        else if (!inBeam && _isRevealed && !_stayRevealedOnceFound)
        {
            Hide();
        }
    }

    public void Reveal()
    {
        if (_isRevealed) return;
        _isRevealed = true;

        SetRevealedVisuals(true);
        gameObject.layer = LayerMask.NameToLayer(_interactableLayerName);

        GameEvents.RaiseHiddenClueRevealed(this);
        // Note: the InteractableItem/ClueCard flow still fires separately when
        // the player then presses E on it — Reveal() only makes it findable.
    }

    private void Hide()
    {
        _isRevealed = false;
        SetRevealedVisuals(false);
        gameObject.layer = LayerMask.NameToLayer("Hidden");
    }

    private void SetRevealedVisuals(bool revealed)
    {
        if (_visual != null) _visual.SetActive(revealed);
        if (_interactCollider != null) _interactCollider.enabled = revealed;
    }
}