using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    [SerializeField] bool GoNextScene = true;
    [SerializeField] string nextSceneName = "NextScene"; // Name of the next scene to load
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GoNextScene)
            {
                // Load the next scene
             SceneController.Instance.LoadScene();
            }
            else
            {
                
             SceneController.Instance.LoadScene(nextSceneName);
            }
        }
    }
}
