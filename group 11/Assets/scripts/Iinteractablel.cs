using UnityEngine;

/// <summary>
/// Every interactable in the game implements this: the pie, notes, doors,
/// safes, hidden scanner clues, NPCs, etc. The raycast in PlayerInteractor
/// only ever talks to this interface — it never knows what kind of object
/// it's looking at.
/// </summary>
public interface IInteractable
{
    // Text shown in the "Press E to ..." prompt. Return null/empty to hide the prompt.
    string GetPrompt();

    // Called when the player presses the Interact action while looking at this object.
    void Interact(PlayerInteractor player);
}
