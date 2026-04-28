using UnityEngine;
using UnityEngine.InputSystem;

public class InteractuableObjects : InteractionEmission
{

    private bool active = false;

    private bool touchPlayer = false;

    [SerializeField] Key interactionKey;

    [SerializeField] PuzzleManager manager;

    private bool locked = false;

    private Animation animations;
    private AudioSource[] audios;

    void Start()
    {
       animations = GetComponent<Animation>();
       audios = GetComponents<AudioSource>();
        SetMaterials();
    }

    void Update()
    {
        if(locked)
        {
            return;
        }

        if (touchPlayer && Keyboard.current[interactionKey].wasPressedThisFrame && !active)
        {
            PressButton();
            if (manager != null)
                manager.ButtonChecker();
        }
    }

    void PressButton()
    {
        animations.Play("wheel|wheelUp");
        audios[0].Play();
        active = true;
    }


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchPlayer = false;
            DeActivateEmission();
        }
    }

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

    public bool Active()
    {
        return active;
    }

    public void LockButton()
    {
        locked = true;
    }


    public void ResetButtons()
    {
        active = false;
        animations.Play("wheel|wheelDown");
        audios[1].Play();
    }

    
}
