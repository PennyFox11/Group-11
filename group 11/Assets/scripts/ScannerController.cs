using Unity.VectorGraphics.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

//Akhona Khoali
//This script is to toggle on and off the toggle light of the scanner(spotlight) and to get the scanners position, direction, range and angle for the HiddenClue script to use to check if the clue is in the beam of the scanner.
public class ScannerController : MonoBehaviour
{
    [Header("Scanner Light")]
    [Tooltip("A Spot Light, usually parented under the camera so it points where you look.")]
    [SerializeField] private Light scannerLight;

    [Header("Feedback (optional)")]
    [Tooltip("Optional beam/particle VFX that plays alongside the light while scanning.")]
    [SerializeField] private ParticleSystem scanBeamVFX;

    public bool IsOn { get; private set; } = false;

    private void Awake()
    {
        if (scannerLight != null)
        {
            scannerLight.enabled = false;
        }
    }

    // shows that the the the event sysetem is to be added to toggle the light on and off when the player presses the toggle scanner button(F) in the player input map
    public void OnToggleScanner(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        IsOn = !IsOn;

        if (scannerLight != null)
        {
            scannerLight.enabled = IsOn;
        }

        if (IsOn)
        {
            scanBeamVFX?.Play();
            AudioManager.Instance?.PlayLoop(SFXType.ScannerHum);
        }
        else
        {
            scanBeamVFX?.Stop();
            AudioManager.Instance?.StopLoop(SFXType.ScannerHum);
        }
    }

    // These let HiddenClue objects check whether they're inside the beam to be visible to the naked eye.
    public Vector3 GetOrigin() => scannerLight != null ? scannerLight.transform.position : transform.position;
    public Vector3 GetForward() => scannerLight != null ? scannerLight.transform.forward : transform.forward;
    public float GetRange() => scannerLight != null ? scannerLight.range : 10f;
    public float GetSpotAngle() => scannerLight != null ? scannerLight.spotAngle : 30f;

    /// <summary>
    /// Convenience check for HiddenClue: is worldPoint inside the scanner's
    /// cone right now? Combines range + angle so each clue doesn't have to
    /// re-derive the cone math itself. Does NOT check line-of-sight
    /// (occlusion) — add a Physics.Linecast here if you want walls to block
    /// the beam.
    /// </summary>
    public bool IsPointInBeam(Vector3 worldPoint)
    {
        if (!IsOn) return false;

        Vector3 origin = GetOrigin();
        Vector3 toPoint = worldPoint - origin;
        float distance = toPoint.magnitude;

        if (distance > GetRange()) return false;

        float angleToPoint = Vector3.Angle(GetForward(), toPoint);
        return angleToPoint <= GetSpotAngle() * 0.5f;
    }
}