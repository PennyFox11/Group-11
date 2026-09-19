using UnityEngine;

public enum EvidenceType
{
    CharacterProfile,
    LocationInfo,
    Photograph,
    Document,
    CallInfo,
    FingerprintInfo,
    StickyNote
}

/// <summary>
/// Content for a clue card on the cork board. Separate from ItemData because
/// not every ItemData is a clue, and clues can also come from puzzles (no
/// physical pickup at all).
/// Create via: Assets > Create > Detective > Clue Data
/// </summary>
[CreateAssetMenu(fileName = "NewClueData", menuName = "Detective/Clue Data")]
public class ClueData : ScriptableObject
{
    public string clueId;              // unique string id, e.g. "frank_fingerprint_pie"
    public string cardTitle;           // "Frank's Fingerprint on Pie"
    [TextArea(3, 8)] public string cardDescription; // the deduction text
    public EvidenceType evidenceType;
    public Sprite cardImage;           // optional photograph/document image

    [Header("Extended Info (optional, per evidence type)")]
    public string personName;
    public string address;
    public string occupation;
    [TextArea] public string relationships;
    [TextArea] public string associatedLocations;
}