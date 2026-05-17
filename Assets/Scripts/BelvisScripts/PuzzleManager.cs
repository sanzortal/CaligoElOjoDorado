using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.Netcode;
public class PuzzleManager : NetworkBehaviour
{


    [SerializeField] float timer;

    private bool puzzleActive = false;

    [SerializeField] List<InteractuableObjects> buttons;

    private float currentTime;


    private bool puzzleCompleted = false;

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


    void Update()
    {
        if(puzzleActive)
        {
            currentTime = currentTime - Time.deltaTime;

            if(currentTime <= 0)
            {
                ResetPuzzle();
            }
        }
    }


    public void ButtonChecker()
    {

        if(puzzleCompleted)
        {
            return;
        }

        if (!puzzleActive)
        {
            puzzleActive = true;
            currentTime = timer;
        }

        bool allPressed = true;
        foreach (var button in buttons) 
        { 
            if(!button.Active())
            {
                allPressed = false;
            }
        }

        if(allPressed)
        {
            puzzleCompleted = true;
            
            puzzleActive = false;

            foreach (var button in buttons)
            {
                button.LockButton();
            }

            DoorOpenClientRpc();
        }
    }

      

    void ResetPuzzle()
    {
        if(puzzleCompleted)
        {
            return;
        }

        puzzleActive = false;
        foreach (var button in buttons)
        {
            if (button.Active())
            {
                button.ResetButtonsClientRpc();
            }
        }
    }
}
