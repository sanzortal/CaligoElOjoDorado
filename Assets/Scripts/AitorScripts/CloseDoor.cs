using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    private AudioSource[] doorSounds;
    private Animation doorAnimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorSounds = door.GetComponents<AudioSource>();
        doorAnimation = door.GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();

        if (playerController)
        {
            doorSounds[1].Play();
            doorAnimation.Play("Door|Close");
        }
    }
}
