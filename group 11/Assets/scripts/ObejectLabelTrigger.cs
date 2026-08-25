using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//Akhona Khoali
//this script is for the to displayer a label(text) when the player collides it it

//for this to work the mesh collider had to be disabled and a box collider was used to replace it. This unfortunaly resulted in the plyer waking through the object. This will re refined in the next prototype. I did not want to manually add new colliders

[RequireComponent(typeof(Collider))]
public class ObjectLabelTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";

    [Header("Label")]
    [Tooltip("The text shown when the player is close, e.g. 'This is a chair.'")]
    [TextArea(1, 3)]
    [SerializeField] private string labelText = "This is a chair.";

    [Tooltip("The UI GameObject to show/hide (e.g. a Panel holding the Text).")]
    [SerializeField] private GameObject labelPanel;

    [Tooltip("The UI Text component that displays the label. Optional if you're only toggling a fixed panel.")]
    [SerializeField] private Text labelTextComponent;

    [Header("Extra Events (optional)")]
    public UnityEvent onLabelShow;
    public UnityEvent onLabelHide;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (labelTextComponent != null)
        {
            labelTextComponent.text = labelText;
        }

        if (labelPanel != null)
        {
            labelPanel.SetActive(true);
        }

        onLabelShow?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (labelPanel != null)
        {
            labelPanel.SetActive(false);
        }

        onLabelHide?.Invoke();
    }
}