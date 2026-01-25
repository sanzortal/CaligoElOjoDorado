using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    private AudioSource doorSound;
    private Animation doorAnimation;



    void Start()
    {
        doorSound = this.gameObject.GetComponent<AudioSource>();
        doorAnimation = this.gameObject.GetComponent<Animation>();
    }

    public void DoorOpen()
    {
        doorSound.Play();
        doorAnimation.Play("Door|Open");
    }
}
