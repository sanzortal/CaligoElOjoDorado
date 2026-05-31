using System.Collections;
using UnityEngine;

public class HumbleEyeController : MonoBehaviour
{
    //max rotations
    [SerializeField]
    private Vector3 rotationA = new Vector3(0, 207.559799f, 0);

    [SerializeField]
    private Vector3 rotationB = new Vector3(0, 152.788376f, 0);

    //speed rotation and wait time
    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float waitTime = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EyeMovementRoutine());
    }

    //coroutine that calls the rotation waiting x seconds
    IEnumerator EyeMovementRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(RotateTo(rotationA));
            yield return new WaitForSeconds(waitTime);
            
            yield return StartCoroutine(RotateTo(rotationB));
            yield return new WaitForSeconds(waitTime);
            
        }
    }

    //rotate the eye to the angles that were established before
    IEnumerator RotateTo(Vector3 targetRotation)
    {
        while (Vector3.Distance(transform.eulerAngles, targetRotation) > 0.5f)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, 
                                    Time.deltaTime * speed);
            yield return null;
        }
    }
}
