using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.Netcode;
public class PuzzleManager : NetworkBehaviour
{

    //checks
    [SerializeField] float timer;

    private bool puzzleActive = false;

    //puzzle buttons
    [SerializeField] List<InteractuableObjects> buttons;

    //more checks
    private float currentTime;

    private bool puzzleCompleted = false;

    //objects to use
    [SerializeField] GameObject door;
    private AudioSource doorSound;
    private Animation doorAnimation;


    //get the sound and the animation of the dorr
    void Start()
    {
        doorSound = door.GetComponent<AudioSource>();
        doorAnimation = door.GetComponent<Animation>();
    }

    //open the door also for the clients
    [ClientRpc]
    public void DoorOpenClientRpc()
    {
        doorSound.Play();
        doorAnimation.Play("Door|Open");
    }

    //check if the time the player has taken to complete the puzzle is less than the maximum time
    void Update()
    {
        if(puzzleActive)
        {
            currentTime = currentTime - Time.deltaTime;

            //if the time ran out reset the puzzle
            if(currentTime <= 0)
            {
                ResetPuzzle();
            }
        }
    }

    //checks if is possible to press the buttons
    public void ButtonChecker()
    {
        //if the puzzle is completed return
        if(puzzleCompleted)
        {
            return;
        }

        //if the puzzle is inactive starts the timer 
        if (!puzzleActive)
        {
            puzzleActive = true;
            currentTime = timer;
        }

        //checks the buttons that are pressed
        bool allPressed = true;
        foreach (var button in buttons) 
        { 
            if(!button.Active())
            {
                allPressed = false;
            }
        }

        //if all the buttons are pressed, locks the puzzle and opens the door
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

      
    //reset all the buttons to normal
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
