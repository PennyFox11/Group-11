using UnityEngine;

/// <summary>
/// Base class for every phone puzzle (Call Log, Timeline, Contradiction, etc).
/// Each puzzle type is a separate subclass, but they all plug into the same
/// phone UI shell and the same unlock/solve event flow. See CallLogPuzzle
/// for a worked example.
/// </summary>
public abstract class PuzzleBase : MonoBehaviour
{
    [SerializeField] protected string _puzzleId;
    [Tooltip("This puzzle only becomes available on the phone once this clue has been found.")]
    [SerializeField] protected ClueData _requiredClueToUnlock;
    [Tooltip("Optional: solving this puzzle adds a new clue to the cork board.")]
    [SerializeField] protected ClueData _resultClue;

    public bool IsUnlocked { get; private set; }
    public bool IsSolved { get; private set; }

    protected virtual void OnEnable()
    {
        if (_requiredClueToUnlock == null)
        {
            IsUnlocked = true; // no prerequisite
        }
        else
        {
            GameEvents.OnClueDetected += HandleClueDetected;
        }
    }

    protected virtual void OnDisable()
    {
        GameEvents.OnClueDetected -= HandleClueDetected;
    }

    private void HandleClueDetected(ClueData clue)
    {
        if (clue == _requiredClueToUnlock)
        {
            IsUnlocked = true;
        }
    }

    // Call this from the puzzle's UI when the player submits an answer.
    protected void Submit(bool isCorrect)
    {
        if (isCorrect)
        {
            IsSolved = true;
            AudioManager.Instance.Play(SFXType.PuzzleCorrect);
            GameEvents.RaisePuzzleSolved(_puzzleId);

            if (_resultClue != null)
            {
                GameEvents.RaiseClueDetected(_resultClue);
            }
            OnCorrect();
        }
        else
        {
            AudioManager.Instance.Play(SFXType.PuzzleWrong);
            GameEvents.RaisePuzzleFailed(_puzzleId);
            OnIncorrect();
        }
    }

    protected abstract void OnCorrect();
    protected abstract void OnIncorrect();
}