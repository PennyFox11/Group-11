using UnityEngine;
using UnityEngine.InputSystem;

public class ScannerController : MonoBehaviour
{
    [Header("Scanner Light")]
    [Tooltip("A Spot Light, usually parented under the camera so it points where you look.")]
    [SerializeField] private Light scannerLight;

    public bool IsOn { get; private set; } = false;

    private void Awake()
    {
        if (scannerLight != null)
        {
            scannerLight.enabled = false;
        }
    }

    // Wire this to your Scanner toggle Input Action -> Invoke Unity Events -> Performed
    public void OnToggleScanner(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        IsOn = !IsOn;

        if (scannerLight != null)
        {
            scannerLight.enabled = IsOn;
        }
    }

    // These let HiddenClue objects check whether they're inside the beam
    public Vector3 GetOrigin() => scannerLight != null ? scannerLight.transform.position : transform.position;
    public Vector3 GetForward() => scannerLight != null ? scannerLight.transform.forward : transform.forward;
    public float GetRange() => scannerLight != null ? scannerLight.range : 10f;
    public float GetSpotAngle() => scannerLight != null ? scannerLight.spotAngle : 30f;
}