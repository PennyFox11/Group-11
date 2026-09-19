using System.Collections;
using TMPro;
using Unity.VectorGraphics.Editor;
using UnityEngine;

/// <summary>
/// The "Clue Detected" popup at the bottom of the screen. Subscribes to
/// OnClueDetected so any clue anywhere in the game triggers it automatically.
/// </summary>
public class NotificationUI : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private float _displayDuration = 2.5f;

    private Coroutine _hideRoutine;

    private void OnEnable()
    {
        GameEvents.OnClueDetected += HandleClueDetected;
        GameEvents.OnAreaUnlocked += HandleAreaUnlocked;
    }

    private void OnDisable()
    {
        GameEvents.OnClueDetected -= HandleClueDetected;
        GameEvents.OnAreaUnlocked -= HandleAreaUnlocked;
    }

    private void HandleClueDetected(ClueData clue)
    {
        Show("Clue Detected");
        AudioManager.Instance.Play(SFXType.ClueDetected);
    }

    private void HandleAreaUnlocked(string areaId)
    {
        Show("Door Unlocked");
        AudioManager.Instance.Play(SFXType.DoorUnlock);
    }

    private void Show(string text)
    {
        _label.text = text;
        _root.SetActive(true);

        if (_hideRoutine != null) StopCoroutine(_hideRoutine);
        _hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(_displayDuration);
        _root.SetActive(false);
    }
}