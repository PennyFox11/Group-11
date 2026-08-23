using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string scenetoload;



public void ChangeSceneNow()
    {
        SceneManager.LoadScene(scenetoload);
    }
public void QuitGame()
   {
       Application.Quit();
   }

}


    
