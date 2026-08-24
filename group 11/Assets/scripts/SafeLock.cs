using UnityEngine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class SafeLock : MonoBehaviour
{
    [Header("Code Settings")]
    [Tooltip("The correct code, digits only, e.g. '1234'.")]
    [SerializeField] private string correctCode = "1234";

    [Header("UI References")]
    [SerializeField] private GameObject keypadPanel;
    [SerializeField] private Text enteredCodeText;
    [Tooltip("Optional - shows a message like 'Incorrect' when the code is wrong.")]
    [SerializeField] private Text feedbackText;

    [Header("Events")]
    [Tooltip("Fires once, when the correct code is entered. Wire this to your door/safe-door opening script.")]
    public UnityEvent onUnlock;

    private string enteredCode = "";
    private bool isActive = false;
    private bool isUnlocked = false;

    // Wire this to InteractionZone's On Interact event
    public void OpenKeypad()
    {
        if (isUnlocked) return;

        isActive = true;
        enteredCode = "";
        UpdateDisplay();

        if (keypadPanel != null) keypadPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Wire this to InteractionZone's On Zone Exit event, so walking away closes the keypad
    public void CloseKeypad()
    {
        isActive = false;

        if (keypadPanel != null) keypadPanel.SetActive(false);

        if (!isUnlocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (!isActive || isUnlocked) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit0Key.wasPressedThisFrame) AddDigit('0');
        if (Keyboard.current.digit1Key.wasPressedThisFrame) AddDigit('1');
        if (Keyboard.current.digit2Key.wasPressedThisFrame) AddDigit('2');
        if (Keyboard.current.digit3Key.wasPressedThisFrame) AddDigit('3');
        if (Keyboard.current.digit4Key.wasPressedThisFrame) AddDigit('4');
        if (Keyboard.current.digit5Key.wasPressedThisFrame) AddDigit('5');
        if (Keyboard.current.digit6Key.wasPressedThisFrame) AddDigit('6');
        if (Keyboard.current.digit7Key.wasPressedThisFrame) AddDigit('7');
        if (Keyboard.current.digit8Key.wasPressedThisFrame) AddDigit('8');
        if (Keyboard.current.digit9Key.wasPressedThisFrame) AddDigit('9');

        if (Keyboard.current.backspaceKey.wasPressedThisFrame) RemoveLastDigit();
    }

    private void AddDigit(char digit)
    {
        if (enteredCode.Length >= correctCode.Length) return;

        enteredCode += digit;
        UpdateDisplay();

        if (enteredCode.Length == correctCode.Length)
        {
            CheckCode();
        }
    }

    private void RemoveLastDigit()
    {
        if (enteredCode.Length == 0) return;

        enteredCode = enteredCode.Substring(0, enteredCode.Length - 1);
        UpdateDisplay();
    }

    private void CheckCode()
    {
        if (enteredCode == correctCode)
        {
            Unlock();
        }
        else
        {
            if (feedbackText != null) feedbackText.text = "Wrong code";
            enteredCode = "";
            UpdateDisplay();
        }
    }

    private void Unlock()
    {
        isUnlocked = true;
        isActive = false;

        if (feedbackText != null) feedbackText.text = "Correct";

        StartCoroutine(CloseAfterDelay());

        onUnlock?.Invoke();
    }

    private System.Collections.IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (keypadPanel != null) keypadPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateDisplay()
    {
        if (enteredCodeText != null) enteredCodeText.text = enteredCode;
    }
}