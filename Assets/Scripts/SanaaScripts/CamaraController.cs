using UnityEngine;
using UnityEngine.UIElements;

public class CamaraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 0.025f;
    [SerializeField] private Vector3 offset;

    public Transform minLimitTransform;
    public Transform maxLimitTransform;

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float clampX = Mathf.Clamp(
            smoothPosition.x,
            minLimitTransform.position.x,
            maxLimitTransform.position.x
        );

        float clampY = Mathf.Clamp(
            smoothPosition.y,
            minLimitTransform.position.y,
            maxLimitTransform.position.y
        );

        transform.position = new Vector3(clampX, clampY, offset.z);
    }

}
