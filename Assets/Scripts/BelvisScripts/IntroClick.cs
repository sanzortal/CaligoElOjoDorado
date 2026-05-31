using UnityEngine;

public class IntroClick : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] SceneLoader loader;

    //check if the player clicks in the screen and change the scene
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            loader.LoadScene(nextSceneName);
        }
    }
}
