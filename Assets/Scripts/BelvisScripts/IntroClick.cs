using UnityEngine;

public class IntroClick : MonoBehaviour
{
    [SerializeField] string nextSceneName;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SceneLoader.Instance.LoadScene(nextSceneName);
        }
    }
}
