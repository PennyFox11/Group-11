using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Lines")]
    [Tooltip("Each entry is shown one at a time. Pressing E again shows the next line.")]
    [TextArea(2, 4)]
    [SerializeField]
    private string[] lines = new string[]
    {
        "Detective hurry!!!.....",
        "Frank has been found dead.",
        "Quickly find the code to his safe. Press E to intect and pick up objects",
        "Press Q to scan for fingerprints",
        "When you have found the code go unlock the safe in his bedroom to find a gun",
        "When you have found the finger prints check your phone using R and pick the right fingerprint",
        "Goodluck on your mission"
    };

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueTextComponent;

    private int currentLineIndex = -1;
    private bool isActive = false;

    // Wire this to InteractionZone's On Interact event
    public void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= lines.Length)
        {
            EndDialogue();
            return;
        }

        if (!isActive)
        {
            isActive = true;
            if (dialoguePanel != null) dialoguePanel.SetActive(true);
        }

        if (dialogueTextComponent != null)
        {
            dialogueTextComponent.text = lines[currentLineIndex];
        }
    }

    // Wire this to InteractionZone's On Zone Exit event, so walking away closes the box
    public void EndDialogue()
    {
        isActive = false;
        currentLineIndex = -1;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}