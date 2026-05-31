using UnityEngine;

public class MirrorCameraController : MonoBehaviour
{
    [SerializeField] Camera mirrorCamera;
    [SerializeField] bool activate;
    [SerializeField] GameObject otherTrigger;

    //activate/deactivate the mirror depending on whether the player is in the room or not
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (activate)
            {
                mirrorCamera.gameObject.SetActive(true);
            }
            else
            {
                mirrorCamera.gameObject.SetActive(false);
            }

            this.gameObject.SetActive(false);
            otherTrigger.SetActive(true);
        }
    }
}
