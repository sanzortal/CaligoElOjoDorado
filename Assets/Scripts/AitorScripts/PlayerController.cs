
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float movementSpeed;
    private float initialSpeed;
    private float maxSpeed;
    [SerializeField] float aceleration;
    [SerializeField] float rotationSpeed;
    private bool isMoving;

    //Control keys
    [SerializeField] Key fowardKey;
    [SerializeField] Key backwardKey;
    [SerializeField] Key leftKey;
    [SerializeField] Key rightKey;

    //Jump
    [SerializeField] float jumpForce;
    private bool inAir;
    [SerializeField] LayerMask mask;
    [SerializeField] float airDistance;

    //crouch
    private bool isCrouching;
    [SerializeField] float crouchSpeed;

    //slide
    private Vector3 slideVector;
    [SerializeField] float slideForce;

    //RigidBody
    private Rigidbody rb;

    //Collider
    private BoxCollider bc;
    private Vector3 initSize;
    private Vector3 initCenter;

    //emotions
    public enum emotions
    {
        NORMAL,
        SAD,
        HAPPY,
        ANGRY,
        CALM
    }

    private emotions emotion;
    private bool differentEmotion;

    private SceneInteractableBehaviour interactableObject;
    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        bc = this.gameObject.GetComponent<BoxCollider>();
        initSize = bc.size;
        initCenter = bc.center;
        initialSpeed = movementSpeed;
        maxSpeed = movementSpeed + 4;
        isCrouching = false;
        isMoving = false;
        slideVector = Vector3.zero;
        emotion = emotions.NORMAL;
        differentEmotion = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            SadEmotion();
        }

        slideVector = Move();
        InAir();
        if (emotion == emotions.NORMAL)
        {
            if (Keyboard.current.leftShiftKey.isPressed && isMoving && !inAir)
            {
                Run();

                if (Keyboard.current.leftCtrlKey.wasPressedThisFrame && !inAir && movementSpeed > initialSpeed + 2f)
                {
                    slide(slideVector);
                }
            }

            if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
            {
                crouched();
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame && !inAir && !isCrouching)
            {
                Jump();
            }

            if (Keyboard.current.leftCtrlKey.wasPressedThisFrame && !inAir) 
            {
                Crouch();
            }
            else if (Keyboard.current.leftCtrlKey.wasReleasedThisFrame)
            {
                StandUp();
            }
        }

        if (interactableObject != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            interactableObject.Interact(this.emotion);
        }

    }

    Vector3 Move()
    {
        Vector3 moveDirection = CalculateMoveDirection();

        transform.position = transform.position + moveDirection * movementSpeed * Time.deltaTime;

        if (moveDirection.magnitude != 0)
        {
            LookAt(moveDirection);

            return moveDirection;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void LookAt(Vector3 lookDirection)
    {
        Quaternion targetRotation;
        targetRotation = Quaternion.LookRotation(lookDirection); //Esto lo que hace es calcular la rotación que debería tener si estuviese girado para donde va realmente

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

        if (moveVector == Vector3.zero)
        {
            isMoving = false;

            if (!differentEmotion)
            {
                crouched();
            }
        }
        else
        {
            isMoving = true;
        }

        moveNormalized = moveVector.normalized;

        return moveNormalized;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        inAir = true;
    }

    private void InAir()
    {
        Debug.DrawRay(transform.position, Vector3.down * airDistance, Color.green);

        if (Physics.Raycast(transform.position, Vector3.down, airDistance, mask))
        {
            inAir = false;
        }
        else
        {
            inAir = true;
        }
    }

    private void Crouch()
    {
        bc.size = new Vector3(initSize.x, initSize.y / 2, initSize.z);
        bc.center = new Vector3(initCenter.x, -(bc.size.y) / 2, initCenter.z);
        movementSpeed = crouchSpeed;
        isCrouching = true;
    }

    private void StandUp()
    {
        bc.size = initSize;
        bc.center = initCenter;
        movementSpeed = initialSpeed;
        isCrouching = false;
    }

    private void Run()
    {
        float max = maxSpeed;

        if (isCrouching)
        {
            max = maxSpeed - (initialSpeed-crouchSpeed);
        }

        if (movementSpeed < max)
        {
            movementSpeed += movementSpeed * aceleration * Time.deltaTime;
        }
    }

    void crouched()
    {
        if (!isCrouching)
        {
            movementSpeed = initialSpeed;
        }
        else
        {
            Crouch();
        }
    }

    void slide(Vector3 moveDirection)
    {
        rb.AddForce(moveDirection * slideForce, ForceMode.Impulse);
    }

    void SadEmotion()
    {
        if (isCrouching)
        {
            StandUp();
        }

        if (!differentEmotion)
        {
            emotion = emotions.SAD;
            differentEmotion = true;
            movementSpeed = initialSpeed - 2;
        }
        else
        {
            emotion = emotions.NORMAL;
            differentEmotion = false;
            movementSpeed = initialSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        SceneInteractableBehaviour aux = collision.gameObject.GetComponent<SceneInteractableBehaviour>(); 

        if (aux != null )
        {
            interactableObject = aux;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        SceneInteractableBehaviour aux = collision.gameObject.GetComponent<SceneInteractableBehaviour>();

        if (aux != null)
        {
            interactableObject = null;
        }
    }
}
