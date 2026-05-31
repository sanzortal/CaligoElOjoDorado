using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionsManager : MonoBehaviour
{
    //wait time
    [SerializeField] private float transitionTime;

    private Animator animatorController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animatorController = this.gameObject.GetComponentInChildren<Animator>();
    }

    //change the scene
    public IEnumerator LoadScene(int sceneNum)
    {
        //start the transition
        animatorController.SetTrigger("StartTransition");

        //wait x seconds
        yield return new WaitForSeconds(transitionTime);

        //get the name of the current scene through its number.
        string sceneName = SceneUtility.GetScenePathByBuildIndex(sceneNum);
        sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);

        //change the scene
        if (!NetworkManager.Singleton.IsServer) yield break;
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
