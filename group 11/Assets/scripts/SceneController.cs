using UnityEngine;
using UnityEngine.SceneManagement;
//Akhona Khoali 
//This script is to load the next scene when the player enters the exit door trigger collider periodt.A youtube video helped with this script. from same youtube video I used to make the ExitDoor script.
public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
