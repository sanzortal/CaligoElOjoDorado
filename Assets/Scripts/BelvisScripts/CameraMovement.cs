using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    Vector3 relativeDistance;

    //max and min values
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    //check the relative distance to the player
    void Start()
    {
        relativeDistance = transform.position - player.transform.position;
    }

    //move the camera checking the position of the player
    void LateUpdate()
    {
        Vector3 targetPos = player.transform.position + relativeDistance;

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        targetPos.z = Mathf.Clamp(targetPos.z, minZ, maxZ);


        transform.position = targetPos;
    }
}
