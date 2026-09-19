using UnityEngine;

/// <summary>
/// A door that's locked (shows "Locked" prompt, does nothing on interact)
/// until GameEvents.OnAreaUnlocked fires with a matching areaId. Once
/// unlocked, opens/teleports the player on interact.
/// For a single-scene "respawn" layout, set _teleportTarget to the office
/// spawn point transform.
/// </summary>
public class DoorManager : MonoBehaviour, IInteractable
{
    [SerializeField] private string _areaId = "apartment_exit";
    [SerializeField] private Transform _teleportTarget; // office spawn point, same scene
    [SerializeField] private Animator _doorAnimator;     // optional, for an open animation

    private bool _isUnlocked;

    private void OnEnable()
    {
        GameEvents.OnAreaUnlocked += HandleAreaUnlocked;
    }

    private void OnDisable()
    {
        GameEvents.OnAreaUnlocked -= HandleAreaUnlocked;
    }

    private void HandleAreaUnlocked(string areaId)
    {
        if (areaId == _areaId)
        {
            _isUnlocked = true;
        }
    }

    public string GetPrompt() => _isUnlocked ? "Open door" : "Locked";

    public void Interact(PlayerInteractor player)
    {
        if (!_isUnlocked) return;

        _doorAnimator?.SetTrigger("Open");

        if (_teleportTarget != null)
        {
            // Simple same-scene "respawn": move the player's CharacterController/root.
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                player.transform.position = _teleportTarget.position;
                player.transform.rotation = _teleportTarget.rotation;
                cc.enabled = true;
            }
            else
            {
                player.transform.position = _teleportTarget.position;
                player.transform.rotation = _teleportTarget.rotation;
            }
        }
    }
}

