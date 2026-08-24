using UnityEngine;

public class HiddenClue : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    // Grabs every renderer in this object AND its children, since the
    // visible mesh is often on a child GameObject rather than this one.
    private Renderer[] clueRenderers;
    private ScannerController scanner;

    private void Awake()
    {
        clueRenderers = GetComponentsInChildren<Renderer>(true);

        if (clueRenderers.Length == 0)
        {
            Debug.LogWarning($"HiddenClue on '{name}' found no Renderer in itself or its children.");
        }

        SetRenderersEnabled(false);
    }

    private void SetRenderersEnabled(bool value)
    {
        foreach (var r in clueRenderers)
        {
            r.enabled = value;
        }
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
        {
            Debug.LogWarning($"[{name}] No GameObject found with tag '{playerTag}'. Check that your player root has this exact tag.");
        }
        else
        {
            scanner = player.GetComponentInChildren<ScannerController>();
            if (scanner == null)
            {
                Debug.LogWarning($"[{name}] Found player object '{player.name}' (tag '{playerTag}'), but no ScannerController on it or its children.");
            }
            else
            {
                Debug.Log($"[{name}] Scanner found successfully on '{scanner.gameObject.name}'.");
            }
        }
    }

    // TEMP DEBUG: logs once a second instead of every frame so the console stays readable.
    private float debugLogTimer = 0f;

    private void Update()
    {
        debugLogTimer += Time.deltaTime;
        bool shouldLog = debugLogTimer >= 1f;
        if (shouldLog) debugLogTimer = 0f;

        if (scanner == null || !scanner.IsOn)
        {
            if (shouldLog) Debug.Log($"[{name}] HIDDEN — scanner null? {scanner == null}, IsOn? {(scanner != null ? scanner.IsOn.ToString() : "n/a")}");
            SetRenderersEnabled(false);
            return;
        }

        Vector3 origin = scanner.GetOrigin();
        Vector3 toClue = transform.position - origin;
        float distance = toClue.magnitude;

        // Too far away for the beam to reach
        if (distance > scanner.GetRange())
        {
            if (shouldLog) Debug.Log($"[{name}] HIDDEN — too far. distance={distance:F1}, range={scanner.GetRange():F1}");
            SetRenderersEnabled(false);
            return;
        }

        // Outside the cone of the spotlight
        float angle = Vector3.Angle(scanner.GetForward(), toClue);
        if (angle > scanner.GetSpotAngle() / 2f)
        {
            if (shouldLog) Debug.Log($"[{name}] HIDDEN — outside cone. angle={angle:F1}, halfSpotAngle={scanner.GetSpotAngle() / 2f:F1}");
            SetRenderersEnabled(false);
            return;
        }

        if (shouldLog) Debug.Log($"[{name}] VISIBLE — distance={distance:F1}, angle={angle:F1}");
        SetRenderersEnabled(true);
    }
}