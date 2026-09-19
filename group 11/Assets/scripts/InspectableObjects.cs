using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class InspectableObject : MonoBehaviour
{
    [Header("Inspection Settings")]
    [SerializeField] private string objectName = "Object";
    [TextArea(2, 5)]
    [SerializeField] private string description = "This is an object.";

    [Header("Events")]
    [Tooltip("Fires when the player presses Q while this is the closest inspectable object.")]
    public UnityEvent onInspectStart;
    [Tooltip("Fires when the player switches to inspecting something else (optional use).")]
    public UnityEvent onInspectEnd;

    public string ObjectName => objectName;
    public string Description => description;

    public void Inspect()
    {
        onInspectStart?.Invoke();
        Debug.Log($"Inspecting: {objectName} - {description}");
    }

    public void StopInspecting()
    {
        onInspectEnd?.Invoke();
    }
}
