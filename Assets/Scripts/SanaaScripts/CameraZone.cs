using Unity.Cinemachine;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public CinemachineCamera vcam;

    //if the player enters in the zone changes the priority of this camera 
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            vcam.Priority = 20;
        }
    }

    //if the player exists the zone change the priority of this camera
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            vcam.Priority = 0;
        }
    }
}
