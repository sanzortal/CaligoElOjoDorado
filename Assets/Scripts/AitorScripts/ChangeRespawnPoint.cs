using System.Collections;
using UnityEngine;

public class ChangeRespawnPoint : MonoBehaviour
{
    [SerializeField] Transform newPoint;
    [SerializeField] GameObject canvaCircle;

    private void OnTriggerEnter(Collider other)
    {     
        DeathsController.ChangeRespawnPoint(newPoint);
        StartCoroutine(showLoadCanvas());
       
    }

    private IEnumerator showLoadCanvas()
    {
        canvaCircle.SetActive(true);

        yield return new WaitForSeconds(3f);

        canvaCircle.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
