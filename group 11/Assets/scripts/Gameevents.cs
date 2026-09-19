using System;
using UnityEngine;

/// <summary>
/// Central static event hub. Gameplay scripts (interactables, puzzles, scanner)
/// fire these; UI, audio, and progression systems subscribe. Nobody needs a
/// direct reference to anybody else.
///
/// If your team grows or you want more Unity-editor-friendly wiring later,
/// swap this for ScriptableObject Event Channels — same pattern, more
/// designer-friendly. For a prototype, static C# events are simplest.
/// </summary>
public static class GameEvents
{
    // Fired when a specific hand-built info panel should be shown for the
    // object just picked up. Each object references its own pre-authored
    // panel GameObject (different layout/text per object) instead of one
    // shared panel reading generic data fields.
    public static event Action<GameObject> OnItemPanelRequested;
    public static void RaiseItemPanelRequested(GameObject panel) => OnItemPanelRequested?.Invoke(panel);

    // Fired when a 3D display model should be spawned on the render rig for
    // the object just picked up (see ItemDisplayController).
    public static event Action<GameObject> OnItemModelRequested;
    public static void RaiseItemModelRequested(GameObject modelPrefab) => OnItemModelRequested?.Invoke(modelPrefab);

    // Fired when an NPC interaction should start a dialogue node. String is
    // the node/key name — your dialogue UI or Yarn Spinner integration reads it.
    public static event Action<string> OnDialogueStarted;
    public static void RaiseDialogueStarted(string nodeName) => OnDialogueStarted?.Invoke(nodeName);

    // Fired only when the picked-up/interacted object IS a clue.
    public static event Action<ClueData> OnClueDetected;
    public static void RaiseClueDetected(ClueData clue) => OnClueDetected?.Invoke(clue);

    // Fired when a note is picked up and should open the fullscreen note reader.
    public static event Action<string> OnNoteOpened; // string = note body text
    public static void RaiseNoteOpened(string noteText) => OnNoteOpened?.Invoke(noteText);

    // Fired when the checklist requirement is met and a door should unlock.
    public static event Action<string> OnAreaUnlocked; // string = door/area id
    public static void RaiseAreaUnlocked(string areaId) => OnAreaUnlocked?.Invoke(areaId);

    // Fired by puzzles on success/failure.
    public static event Action<string> OnPuzzleSolved;   // string = puzzle id
    public static event Action<string> OnPuzzleFailed;   // string = puzzle id
    public static void RaisePuzzleSolved(string puzzleId) => OnPuzzleSolved?.Invoke(puzzleId);
    public static void RaisePuzzleFailed(string puzzleId) => OnPuzzleFailed?.Invoke(puzzleId);

    // Fired by the scanner when it reveals a hidden clue.
    public static event Action<HiddenClue> OnHiddenClueRevealed;
    public static void RaiseHiddenClueRevealed(HiddenClue clue) => OnHiddenClueRevealed?.Invoke(clue);
}