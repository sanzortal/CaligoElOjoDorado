using UnityEngine;

public class IntroClick : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] SceneLoader loader;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            loader.LoadScene(nextSceneName);
        }
    }
}
