using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Tracks which required clues have been found and unlocks an area once
/// all are collected. One of these per "gate" — e.g. one for the apartment
/// door, one for a later safe, etc. Also drives the always-visible checklist
/// UI (bind _checklistText or loop over _requiredClues to build a UI list).
/// </summary>
public class ChecklistManager : MonoBehaviour
{
    [SerializeField] private List<ClueData> _requiredClues;
    [SerializeField] private string _areaId = "apartment_exit";
    [SerializeField] private TMP_Text _checklistText; // simple text list; swap for a proper UI list if you want checkboxes

    private readonly HashSet<string> _found = new HashSet<string>();
    private bool _unlocked;

    private void OnEnable()
    {
        GameEvents.OnClueDetected += HandleClueDetected;
        RefreshUI();
    }

    private void OnDisable()
    {
        GameEvents.OnClueDetected -= HandleClueDetected;
    }

    private void HandleClueDetected(ClueData clue)
    {
        if (_unlocked) return;
        if (!_requiredClues.Contains(clue)) return;

        _found.Add(clue.clueId);
        RefreshUI();

        if (_found.Count >= _requiredClues.Count)
        {
            _unlocked = true;
            GameEvents.RaiseAreaUnlocked(_areaId);
        }
    }

    private void RefreshUI()
    {
        if (_checklistText == null) return;

        var lines = _requiredClues.Select(c =>
            (_found.Contains(c.clueId) ? "[x] " : "[ ] ") + c.cardTitle);

        _checklistText.text = string.Join("\n", lines);
    }
}