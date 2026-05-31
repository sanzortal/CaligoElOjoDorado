using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : NetworkBehaviour
{
    //object variables
    [SerializeField] GameObject door;
    private AudioSource doorSound;
    private Animation doorAnimation;


    //get values
    void Start()
    {
        doorSound = door.GetComponent<AudioSource>();
        doorAnimation = door.GetComponent<Animation>();
    }

    //open the door and play a sound also for the players
    [ClientRpc]
    public void DoorOpenClientRpc()
    {
        doorSound.Play();
        doorAnimation.Play("Door|Open");
    }
}
