using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 relativeDistance;
    void Start()
    {
        relativeDistance = transform.position - player.transform.position;
    }


    void LateUpdate()
    {
        transform.position = player.transform.position + relativeDistance;
    }
}
