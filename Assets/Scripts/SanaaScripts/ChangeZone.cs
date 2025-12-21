using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeZone : MonoBehaviour
{
    [SerializeField] SceneTransitionsManager scMg;
    [SerializeField] bool isNext;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(scMg.LoadScene(numSceneCheck()));
        }
    }

    private int numSceneCheck()
    {
        if (isNext)
        {
            return SceneManager.GetActiveScene().buildIndex + 1;
        }
        else
        {
            return SceneManager.GetActiveScene().buildIndex - 1;
        }
    }
}
