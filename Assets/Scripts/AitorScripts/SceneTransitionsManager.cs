using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionsManager : MonoBehaviour
{
    [SerializeField] private float transitionTime;
    private Animator animatorController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animatorController = this.gameObject.GetComponentInChildren<Animator>();
    }

    public IEnumerator LoadScene(int sceneNum)
    {
        animatorController.SetTrigger("StartTransition");

        yield return new WaitForSeconds(transitionTime);

        string sceneName = SceneUtility.GetScenePathByBuildIndex(sceneNum);
        sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);

        if (!NetworkManager.Singleton.IsServer) yield break;
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
