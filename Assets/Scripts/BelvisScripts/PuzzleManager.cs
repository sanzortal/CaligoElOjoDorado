using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class PuzzleManager : MonoBehaviour
{


    [SerializeField] float timer;

    private bool puzzleActive = false;

    [SerializeField] List<InteractuableObjects> buttons;

    private float currentTime;

    [SerializeField] DoorController door;

    private bool puzzleCompleted = false;


    
    void Start()
    {
        
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
            door.DoorOpen();
            puzzleActive = false;

            foreach (var button in buttons)
            {
                button.LockButton();
            }
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
                button.ResetButtons();
            }
        }
    }
}
