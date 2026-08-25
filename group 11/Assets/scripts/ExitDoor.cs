using UnityEngine;
//Akhona Khoali
//This script is to load the next scene when the player enters the exit door trigger collider periodt.A youtube video helped with this script.

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
