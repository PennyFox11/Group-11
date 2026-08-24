using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Scanner : MonoBehaviour
{
    public Light torchLight;
    public ParticleSystem scanEffect;
    private Controls inputActions;
    private bool isScanning = false;

    void Awake()
    {
        inputActions = new Controls();
    }

    public void Scanning() { 

        void OnEnable()
        {
            inputActions.Player.Scan.performed += ctx => ToggleScanner();
            inputActions.Player.Enable();

        }

        void OnDisable()
        {
            inputActions.Player.Disable();
        }

        void ToggleScanner()
        {
            isScanning = !isScanning;
            torchLight.enabled = isScanning;
            if (isScanning) scanEffect.Play();
            else scanEffect.Stop();
        }

        void OnTriggerEnter(Collider other)
        {
            if (isScanning && other.CompareTag("HiddenObject"))
            {
                MeshRenderer mr = other.GetComponent<MeshRenderer>();
                if (mr != null) mr.enabled = true;
            }

        }
    }
}
