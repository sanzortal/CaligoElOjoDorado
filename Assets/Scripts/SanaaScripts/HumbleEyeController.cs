using System.Collections;
using UnityEngine;

public class HumbleEyeController : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationA = new Vector3(0, 207.559799f, 0);

    [SerializeField]
    private Vector3 rotationB = new Vector3(0, 152.788376f, 0);

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float waitTime = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EyeMovementRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

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

    IEnumerator RotateTo(Vector3 targetRotation)
    {
        while (Vector3.Distance(transform.eulerAngles, targetRotation) > 0.5f)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, Time.deltaTime * speed);
            yield return null;
        }
    }
}
