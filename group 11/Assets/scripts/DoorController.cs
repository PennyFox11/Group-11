using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [Tooltip("How many degrees the door rotates open, around its own Y axis.")]
    [SerializeField] private float openAngle = 90f;
    [Tooltip("How long the door takes to swing open, in seconds.")]
    [SerializeField] private float openDuration = 1f;
    [Tooltip("If true, the door can only ever open once. If false, it will close again when the player leaves (wire EnvironmentalTrigger's On Exit to Close).")]
    [SerializeField] private bool openOnce = true;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentMove;
    private bool isOpen = false;
    private bool hasOpened = false;

    private void Awake()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    // Wire this to EnvironmentalTrigger's On Enter event
    public void Open()
    {
        if (isOpen) return;
        if (openOnce && hasOpened) return;

        hasOpened = true;
        isOpen = true;

        if (currentMove != null) StopCoroutine(currentMove);
        currentMove = StartCoroutine(RotateDoor(openRotation));
    }

    // Optional: wire this to EnvironmentalTrigger's On Exit event if you want the door to close behind the player
    public void Close()
    {
        if (!isOpen) return;
        if (openOnce) return; // once-only doors shouldn't close

        isOpen = false;

        if (currentMove != null) StopCoroutine(currentMove);
        currentMove = StartCoroutine(RotateDoor(closedRotation));
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        Quaternion startRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < openDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / openDuration);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}