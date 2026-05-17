using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : NetworkBehaviour
{
    [SerializeField] GameObject door;
    private AudioSource doorSound;
    private Animation doorAnimation;



    void Start()
    {
        doorSound = door.GetComponent<AudioSource>();
        doorAnimation = door.GetComponent<Animation>();
    }

    [ClientRpc]
    public void DoorOpenClientRpc()
    {
        doorSound.Play();
        doorAnimation.Play("Door|Open");
    }
}
