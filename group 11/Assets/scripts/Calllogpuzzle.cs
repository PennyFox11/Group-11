using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CallLogEntry
{
    public string number;
    public string callerName; // shown after solved, hidden/blank during puzzle if unknown
    public string time;
    public bool isTheAnswer; // mark the ONE entry that's the "blocked government number"
}

/// <summary>
/// Player looks at a list of call log entries and taps the one that's
/// suspicious (e.g. calls to a blocked/government number at an odd hour).
/// Concrete example of PuzzleBase — duplicate this pattern for
/// TimelinePuzzle, ContradictionPuzzle, etc.
/// </summary>
public class CallLogPuzzle : PuzzleBase
{
    [SerializeField] private List<CallLogEntry> _entries;
    [SerializeField] private Transform _entryListContainer;
    [SerializeField] private CallLogEntryUI _entryPrefab;
    [SerializeField] private TMP_Text _feedbackLabel;

    private void Start()
    {
        BuildEntryList();
    }

    private void BuildEntryList()
    {
        foreach (var entry in _entries)
        {
            CallLogEntryUI ui = Instantiate(_entryPrefab, _entryListContainer);
            ui.Setup(entry, OnEntrySelected);
        }
    }

    private void OnEntrySelected(CallLogEntry entry)
    {
        Submit(entry.isTheAnswer);
    }

    protected override void OnCorrect()
    {
        _feedbackLabel.text = "Match found — blocked number, 2:14 AM. Flagged.";
    }

    protected override void OnIncorrect()
    {
        _feedbackLabel.text = "No connection there. Try again.";
    }
}

/// <summary>
/// Small helper component for a single row in the call log UI list.
/// </summary>
public class CallLogEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _numberLabel;
    [SerializeField] private TMP_Text _timeLabel;
    [SerializeField] private Button _button;

    public void Setup(CallLogEntry entry, Action<CallLogEntry> onSelected)
    {
        _numberLabel.text = entry.number;
        _timeLabel.text = entry.time;
        _button.onClick.AddListener(() => onSelected(entry));
    }
}