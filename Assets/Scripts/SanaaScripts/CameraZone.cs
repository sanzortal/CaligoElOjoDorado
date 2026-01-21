using Unity.Cinemachine;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public CinemachineCamera vcam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            vcam.Priority = 20;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            vcam.Priority = 0;
        }
    }
}
