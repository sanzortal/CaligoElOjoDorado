
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
    [SerializeField] LayerMask floorMask;
    [SerializeField] float airDistance;

    //crouch
    private bool tryingToStandUp;
    private float standUpDistance;
    private bool isCrouching;
    [SerializeField] float crouchSpeed;

    //slide
    private Vector3 moveVector;
    [SerializeField] float slideForce;

    //grab
    private bool isGrabbing;

    //RigidBody
    private Rigidbody rb;

    //Collider
    private BoxCollider bc;
    private Vector3 initSize;
    private Vector3 initCenter;


    private Vector3 MoveDirection;

    //Revisar tarea Dani
    public enum InteractionDirection
    {
        None,
        PushForward,
        PullBack,
        MoveLeft,
        MoveRight
    }

    private InteractionDirection currentInteractionDir; //Tarea Dani

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

    //interactable object
    private SceneInteractableBehaviour interactableObject;

    //sounds
    private AudioSource[] audios;
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
        moveVector = Vector3.zero;
        emotion = emotions.NORMAL;
        differentEmotion = false;
        isGrabbing = false;
        audios = GetComponents<AudioSource>();
        tryingToStandUp = false;
        standUpDistance = initSize.y / 2;
    }

    private void FixedUpdate()
    {
        moveVector = Move(MoveDirection);

    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame && !isCrouching)
        {
            SadEmotion();
        }

        MoveDirection = CalculateMoveDirection();
        inAir = InAir();

        if (tryingToStandUp)
        {
            TryToStandUp();
        }

        if (interactableObject != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                interactableObject.Open(this.emotion);
            }

            if (Keyboard.current.qKey.wasPressedThisFrame && !inAir && !isCrouching)
            {
                movementSpeed = initialSpeed - 2;
                isGrabbing = true;
                interactableObject.Move(this.gameObject, this.emotion);
            }

            if (MoveDirection != Vector3.zero)
            {
                if (isGrabbing)
                {
                    currentInteractionDir = CalculateInteractionDirection(MoveDirection, interactableObject.transform); //Tarea Dani
                    interactableObject.playSound();
                }
            }
            else
            {
                interactableObject.stopSound();
            }
        }

        if (Keyboard.current.qKey.wasReleasedThisFrame && isGrabbing)
        {
            isGrabbing = false;
            movementSpeed = initialSpeed;
            interactableObject.stopSound();
            interactableObject.ClearParent();
        }

        if (emotion == emotions.NORMAL && !isGrabbing)
        {
            if (Keyboard.current.leftShiftKey.isPressed && isMoving && !inAir)
            {
                Run();

                if (Keyboard.current.leftCtrlKey.wasPressedThisFrame && !inAir && movementSpeed > initialSpeed + 2f)
                {
                    slide(moveVector);
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
                tryingToStandUp = false;
            }
            else if (Keyboard.current.leftCtrlKey.wasReleasedThisFrame)
            {
                tryingToStandUp = true;
            }
        }

    }

    Vector3 Move(Vector3 moveDirection)
    {
        transform.position = transform.position + moveDirection * movementSpeed * Time.deltaTime;

        if (moveDirection.magnitude != 0)
        {
            if (!isGrabbing)
            {
                LookAt(moveDirection);
            }

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

            if (!differentEmotion && !isGrabbing)
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
        audios[0].Play();
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        inAir = true;
    }

    public bool InAir()
    {
        Debug.DrawRay(transform.position, Vector3.down * airDistance, Color.green);

        if (Physics.Raycast(transform.position, Vector3.down, airDistance, floorMask))
        {
            return false;
        }
        else
        {
            return true;
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

    public emotions getEmotion()
    {
        return this.emotion;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SceneInteractableBehaviour aux = collision.gameObject.GetComponent<SceneInteractableBehaviour>(); 

        if (aux != null)
        {
            interactableObject = aux;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        SceneInteractableBehaviour aux = collision.gameObject.GetComponent<SceneInteractableBehaviour>();

        if (aux != null)
        {
            if (isGrabbing)
            {
                isGrabbing = false;
                movementSpeed = initialSpeed;
            }
            interactableObject.stopSound();
            interactableObject.ClearParent();
            interactableObject = null;           
        }
    }


    //Revisar Tarea Dani
    InteractionDirection CalculateInteractionDirection(Vector3 inputDir,Transform objectTransform)
    {
        if (inputDir == Vector3.zero)
        {
            return InteractionDirection.None;
        }
            

        Vector3 objForward = objectTransform.forward;
        Vector3 objRight = objectTransform.right;

        float forwardDot = Vector3.Dot(inputDir, objForward);
        float rightDot = Vector3.Dot(inputDir, objRight);

        if (forwardDot > 0.7f)
        {
            return InteractionDirection.PushForward;
        }
           

        if (forwardDot < -0.7f)
        {
            return InteractionDirection.PullBack;
        }
            

        if (rightDot > 0.7f)
        {
            return InteractionDirection.MoveRight;
        }
            

        if (rightDot < -0.7f)
        {
            return InteractionDirection.MoveLeft;
        }       
        return InteractionDirection.None;
    }

    //Try to stand up if there is no object above it
    public void TryToStandUp()
    {
        Debug.DrawRay(transform.position, Vector3.up * standUpDistance, Color.black);

        if (!Physics.Raycast(transform.position, Vector3.up, standUpDistance))
        {
            StandUp();
            tryingToStandUp = false;
        }
    }
}
