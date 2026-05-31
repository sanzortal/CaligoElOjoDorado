using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractuableObjects : InteractionEmission
{
    //checks
    private bool active = false;
    private bool locked = false;
    private bool touchPlayer = false;

    //key
    [SerializeField] Key interactionKey;

    //manager
    [SerializeField] PuzzleManager manager;

    private Animation animations;
    private AudioSource[] audios;

    //get values
    void Start()
    {
       animations = GetComponent<Animation>();
       audios = GetComponents<AudioSource>();
       SetMaterials();
       DeActivateEmission();
    }

    void Update()
    {
        if (!IsServer) return;

        //if is locked it cant be activated
        if(locked)
        {
            return;
        }

        //check if the player pressed the interaction key and the button can be pressed
        if (touchPlayer && Keyboard.current[interactionKey].wasPressedThisFrame && !active)
        {
            PressButtonClientRpc();
            if (manager != null)
                manager.ButtonChecker();
        }
    }

    //activate the button and play a sound also for the clients
    [ClientRpc]
    void PressButtonClientRpc()
    {
        animations.Play("wheel|wheelUp");
        audios[0].Play();
        active = true;
    }

    //check if the player is not touching this object and deactivate the emission materials
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchPlayer = false;
            DeActivateEmission();
        }
    }

    //check if the player is touching this object and activate the emission materials
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            touchPlayer = true;

            if (!locked)
            {
                ActivateEmission();
            }
        }

    }

    //return the state of this button/puzzle
    public bool Active()
    {
        return active;
    }

    //lock the button/puzzle 
    public void LockButton()
    {
        locked = true;
    }

    //reset the buttons and play a sound also for the clients
    [ClientRpc]
    public void ResetButtonsClientRpc()
    {
        active = false;
        animations.Play("wheel|wheelDown");
        audios[1].Play();
    }

    
}
