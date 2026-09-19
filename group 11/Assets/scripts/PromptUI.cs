using TMPro;
using UnityEngine;

/// <summary>
/// The small "Press E to pick up (Hot Pie)" text near the crosshair.
/// Just show/hide — no game logic here.
/// </summary>
public class PromptUI : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _label;

    private void Awake()
    {
        Hide();
    }

    public void Show(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            Hide();
            return;
        }
        _root.SetActive(true);
        _label.text = $"[E] {text}";
    }

    public void Hide()
    {
        _root.SetActive(false);
    }
}