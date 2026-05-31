using System.Collections;
using UnityEngine;

public class ChangeRespawnPoint : MonoBehaviour
{
    [SerializeField] Transform newPoint;
    [SerializeField] GameObject canvaCircle;

    //if the player collides with this object, change the respawn point to a new one
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            DeathsController.ChangeRespawnPoint(newPoint);
            StartCoroutine(showLoadCanvas());
        }
    }

    //coroutine that shows the load circle and hides it waiting x seconds
    private IEnumerator showLoadCanvas()
    {
        canvaCircle.SetActive(true);

        yield return new WaitForSeconds(3f);

        canvaCircle.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
