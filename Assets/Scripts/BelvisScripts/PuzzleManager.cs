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
            door.DoorOpen();
        }
    }

      

    void ResetPuzzle()
    {
        puzzleActive = false;
        foreach (var button in buttons)
        {
            button.ResetButtons();
        }
    }
}
