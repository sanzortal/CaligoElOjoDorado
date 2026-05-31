using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeZone : MonoBehaviour
{
    //checks
    [SerializeField] SceneTransitionsManager scMg;
    [SerializeField] bool isNext;

    //if the player enters in the zone, changes the scene to the next/previous one in the scene list
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(scMg.LoadScene(numSceneCheck()));
        }
    }

    //check what the next/previous scene is
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
