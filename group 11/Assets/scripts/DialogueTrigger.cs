using UnityEngine;
using UnityEngine.UI;
//Akhona Khoali
//this script is to trigger the dialogue box when the player presses E and to show the next line of dialogue when pressing E again. The dialogue box is shown using a UI panel and a text component. The dialogue lines are stored in an array of strings and can be set in the inspector. The dialogue box is hidden when the player presses E again after all lines have been shown.
public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Lines")]
    [Tooltip("Each entry is shown one at a time. Pressing E again shows the next line.")]
    [TextArea(2, 4)]
    [SerializeField]
    private string[] lines = new string[]
    {
        //On the inspector you can add lines to the dialogue using the event system so this text is useless for not
        "Detective hurry!!!.....",
        "Frank has been found dead.",
        "Quickly find the code to his safe.Press E open and enter the code",
        "Another think Frank lives alone so there should be one fingerprint in his house.",
        "Find the fingerprint and scan it to get the code to his safe",
        "Press F to scan for fingerprints",
        "When you have found the code go unlock the safe in his bedroom to find a gun",
     
        "Goodluck on your mission"
    };

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueTextComponent;

    private int currentLineIndex = -1;
    private bool isActive = false;

    // shows that an addition to the interation event for the dialogue box and is needed to show the next line of dialogue when pressing E again    
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

    // shows that the dialogue box is no longer active and the current line index is reset to -1 so that the next time the player presses E the dialogue will start from the beginning
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