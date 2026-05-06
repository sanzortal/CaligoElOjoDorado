using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SecondPlayerController : NetworkBehaviour
{
    [SerializeField] GameObject lightPuzzles;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    private Vector3 moveDirection;

    //Control keys
    [SerializeField] Key fowardKey;
    [SerializeField] Key backwardKey;
    [SerializeField] Key leftKey;
    [SerializeField] Key rightKey;
    [SerializeField] Key upKey;
    [SerializeField] Key downKey;
    [SerializeField] Key flashKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
         Move(moveDirection);
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        moveDirection = CalculateMoveDirection();

        if (Keyboard.current[flashKey].wasPressedThisFrame)
        {
            lightPuzzles.SetActive(!lightPuzzles.activeSelf);
        }
    }

    void Move(Vector3 moveDirection)
    {
        transform.position = transform.position + moveDirection * movementSpeed * Time.deltaTime;
        if (moveDirection.magnitude != 0)
        {
            LookAt(moveDirection);
        }
    }

    void LookAt(Vector3 lookDirection)
    {
        Quaternion targetRotation;
        targetRotation = Quaternion.LookRotation(lookDirection); 

        Quaternion newRotation;
        newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = newRotation;
    }

    Vector3 CalculateMoveDirection()
    {
        Vector3 moveVector;
        Vector3 moveNormalized;

        moveVector = new Vector3(0, 0, 0);

        if (Keyboard.current[fowardKey].isPressed)
        {
            moveVector.z = moveVector.z + 1;
        }

        if (Keyboard.current[backwardKey].isPressed)
        {
            moveVector.z = moveVector.z - 1;
        }

        if (Keyboard.current[leftKey].isPressed)
        {
            moveVector.x = moveVector.x - 1;
        }

        if (Keyboard.current[rightKey].isPressed)
        {
            moveVector.x = moveVector.x + 1;
        }

        if (Keyboard.current[upKey].isPressed)
        {
            moveVector.y = moveVector.y + 1;
        }

        if (Keyboard.current[downKey].isPressed)
        {
            moveVector.y = moveVector.y - 1;
        }


        moveNormalized = moveVector.normalized;

        return moveNormalized;
    }

}
