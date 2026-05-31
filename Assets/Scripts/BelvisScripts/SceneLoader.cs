using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //change the scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //close the app
    public void QuitGame()
    {
        Application.Quit();
    }
}
