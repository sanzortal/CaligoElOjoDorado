using UnityEngine;

public class MirrorCameraController : MonoBehaviour
{
    [SerializeField] Camera mirrorCamera;
    [SerializeField] bool activate;
    [SerializeField] GameObject otherTrigger;

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
