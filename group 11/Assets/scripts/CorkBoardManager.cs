using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Owns the cork board Canvas. Listens for clues being detected and spawns
/// a card for each one (only once — duplicates are ignored). Toggled with
/// the "OpenBoard" input action (Q).
///
/// Input System setup: add an "OpenBoard" action (Button) bound to
/// <Keyboard>/q in your Gameplay action map, wire its "performed" event
/// (via PlayerInput Unity Events) to ToggleBoard(), OR use the
/// [Send Messages] OnOpenBoard(InputValue) method below.
/// </summary>
public class CorkBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject _boardRoot;      // the Canvas/panel, inactive by default
    [SerializeField] private RectTransform _cardContainer;
    [SerializeField] private ClueCard _cardPrefab;
    [SerializeField] private InputActionReference _openBoardAction; // optional, if wiring directly

    private readonly HashSet<string> _spawnedClueIds = new HashSet<string>();
    public IReadOnlyCollection<string> CollectedClueIds => _spawnedClueIds;

    private void OnEnable()
    {
        GameEvents.OnClueDetected += HandleClueDetected;
    }

    private void OnDisable()
    {
        GameEvents.OnClueDetected -= HandleClueDetected;
    }

    private void HandleClueDetected(ClueData clue)
    {
        if (_spawnedClueIds.Contains(clue.clueId)) return; // no duplicate cards

        _spawnedClueIds.Add(clue.clueId);

        ClueCard card = Instantiate(_cardPrefab, _cardContainer);
        card.Setup(clue);

        // Simple scatter so cards don't stack exactly on top of each other.
        Vector2 randomOffset = new Vector2(Random.Range(-150f, 150f), Random.Range(-100f, 100f));
        card.GetComponent<RectTransform>().anchoredPosition += randomOffset;
    }

    // Wire to input via PlayerInput Unity Events -> ToggleBoard
    public void ToggleBoard()
    {
        bool willOpen = !_boardRoot.activeSelf;
        _boardRoot.SetActive(willOpen);

        // Pause gameplay look/move while the board is open — recommended:
        // swap action maps here, e.g. inputActions.Gameplay.Disable(); inputActions.UI.Enable();
        Cursor.lockState = willOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = willOpen;
        Time.timeScale = willOpen ? 0f : 1f; // optional: pause the world while reading the board
    }

    // Alternative if using [Send Messages] PlayerInput behaviour:
    public void OnOpenBoard(InputValue value)
    {
        if (value.isPressed) ToggleBoard();
    }
}