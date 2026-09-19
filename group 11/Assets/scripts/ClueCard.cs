using TMPro;
using Unity.VectorGraphics.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A single card on the cork board. Draggable via standard EventSystem
/// drag interfaces (works fine with the new Input System + UI Input Module,
/// no extra input action needed — drag is handled by Unity's UI event system).
/// Has a bottom text field for the player's own notes/conclusions.
/// </summary>
public class ClueCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Display")]
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private TMP_Text _descriptionLabel;
    [SerializeField] private Image _cardImage;

    [Header("Player Notes")]
    [SerializeField] private TMP_InputField _notesField;

    private RectTransform _rect;
    private Canvas _canvas;
    private ClueData _clueData;

    // Persisted player notes, keyed by clue id, survive card destroy/rebuild if needed.
    private static readonly System.Collections.Generic.Dictionary<string, string> _savedNotes
        = new System.Collections.Generic.Dictionary<string, string>();

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(ClueData clue)
    {
        _clueData = clue;
        _titleLabel.text = clue.cardTitle;
        _descriptionLabel.text = clue.cardDescription;
        if (_cardImage != null) _cardImage.sprite = clue.cardImage;

        _notesField.text = _savedNotes.TryGetValue(clue.clueId, out var saved) ? saved : "";
        _notesField.onValueChanged.AddListener(OnNotesChanged);
        _notesField.onSelect.AddListener(_ => AudioManager.Instance.Play(SFXType.Typing));
    }

    private void OnNotesChanged(string value)
    {
        _savedNotes[_clueData.clueId] = value;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling(); // bring to front while dragging
        AudioManager.Instance.Play(SFXType.CardMove);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rect.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Nothing required — per spec, cards don't need to connect/snap to anything.
    }
}