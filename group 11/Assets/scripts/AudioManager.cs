using System;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    Typing,
    CardMove,
    PuzzleCorrect,
    PuzzleWrong,
    NotePickup,
    ClueDetected,
    ScannerHum,
    DoorUnlock
}

[Serializable]
public struct SFXEntry
{
    public SFXType type;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
}

/// <summary>
/// Single point of contact for every sound effect in the game. Any system
/// calls AudioManager.Instance.Play(SFXType.X) — no scattered AudioSource
/// references. Handles one-shots and simple loops (scanner hum).
/// Put this on a persistent GameObject (DontDestroyOnLoad) at the root of
/// your scene, or in a bootstrap scene.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<SFXEntry> _sfxLibrary;
    [SerializeField] private AudioSource _oneShotSource;
    [SerializeField] private AudioSource _loopSource;

    private Dictionary<SFXType, SFXEntry> _lookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _lookup = new Dictionary<SFXType, SFXEntry>();
        foreach (var entry in _sfxLibrary)
        {
            _lookup[entry.type] = entry;
        }
    }

    public void Play(SFXType type)
    {
        if (!_lookup.TryGetValue(type, out var entry) || entry.clip == null) return;
        _oneShotSource.PlayOneShot(entry.clip, entry.volume);
    }

    public void PlayLoop(SFXType type)
    {
        if (!_lookup.TryGetValue(type, out var entry) || entry.clip == null) return;
        _loopSource.clip = entry.clip;
        _loopSource.volume = entry.volume;
        _loopSource.loop = true;
        _loopSource.Play();
    }

    public void StopLoop(SFXType type)
    {
        if (_loopSource.clip == _lookup[type].clip)
        {
            _loopSource.Stop();
        }
    }
}
