
using System.Collections;
using System.Globalization;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    //speed
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
    [SerializeField] float standUpDistance;
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

    [SerializeField] Animator animator;
    private Vector3 moveDirection;

    //sound comprobations
    private bool sliding;
    private bool isRunning;

    //push options
    public enum InteractionDirection
    {
        None,
        PushForward,
        PullBack,
        MoveLeft,
        MoveRight
    }

    private InteractionDirection currentInteractionDir;

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
    private PlayerSoundController soundController;

    //get and set values
    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        bc = this.gameObject.GetComponent<BoxCollider>();
        initSize = bc.size;
        initCenter = bc.center;
        initialSpeed = movementSpeed;
        maxSpeed = movementSpeed + 2;
        isCrouching = false;
        isMoving = false;
        moveVector = Vector3.zero;
        emotion = emotions.NORMAL;
        differentEmotion = false;
        isGrabbing = false;
        soundController = GetComponent<PlayerSoundController>();
        tryingToStandUp = false;
        
        sliding = false;
        isRunning = false;

        //set the targets of all the cameras to the player
        CinemachineCamera[] cameras = 
            Object.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);

        foreach (CinemachineCamera cam in cameras)
        {
            cam.Follow = transform;
            cam.LookAt = transform;
        }

    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        moveVector = Move(moveDirection);

    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        //check if the player pressed m key and can change to the sad emotion
        if (Keyboard.current.mKey.wasPressedThisFrame && !isCrouching)
        {
            SadEmotion();
        }
        
        //calculate the move direction
        moveDirection = CalculateMoveDirection();

        //check if is in air
        inAir = InAir();

        //if the player wants to stand up, check if he can
        if (tryingToStandUp)
        {
            TryToStandUp();
        }
        
        //check if the player is colliding with an interactable object
        if (interactableObject != null)
        {
            //if the player is in the ground and is not crouching grab the object
            if (Keyboard.current.qKey.wasPressedThisFrame && !inAir && !isCrouching)
            {
                isRunning = false;
                movementSpeed = initialSpeed - 2;
                isGrabbing = true;
                animator.SetBool("isGrabbing", true);
                interactableObject.Move(this.gameObject, this.emotion);
                
            }

            //check if the player is moving or not
            if (moveDirection != Vector3.zero)
            {
                //if the player is grabbing calculate de grab direction and play a sound
                if (isGrabbing)
                {
                    currentInteractionDir = CalculateInteractionDirection(moveDirection, 
                                            interactableObject.transform); //Tarea Dani
                    animator.SetInteger("PushDirection", (int)currentInteractionDir);
                    interactableObject.playSoundClientRpc();
                }
            }
            else
            {
                //if the player is grabbing stopp the animations and sound
                if (isGrabbing)
                {
                    animator.SetInteger("PushDirection", 0);
                }
                interactableObject.stopSoundClientRpc();
            }
        }

        //check if the player stops grabbing an object
        if (Keyboard.current.qKey.wasReleasedThisFrame && isGrabbing)
        {
            //reset values
            isGrabbing = false;
            movementSpeed = initialSpeed;

            //reset animations
            animator.SetBool("isGrabbing", false);
            animator.SetInteger("PushDirection", 0);

            //stop sounds and clear the parent of the object
            interactableObject.stopSoundClientRpc();
            interactableObject.ClearParent();
            interactableObject.ActivateEmission();
        }

        //check if the player is in the correct emotion and is not grabbing
        if (emotion == emotions.NORMAL && !isGrabbing)
        {
            //if the player press left shift he starts to run
            if (Keyboard.current.leftShiftKey.isPressed && isMoving && !inAir)
            {
                Run();
                animator.SetBool("isRunning", true);

                //check if the player can slide
                if (Keyboard.current.leftCtrlKey.wasPressedThisFrame && !inAir && 
                    movementSpeed > initialSpeed + 2f)
                {
                    slide(moveVector);
                }
            }

            //check if the player release the left sift key
            if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
            {
                //stop running
                isRunning = false;
                crouched();
                animator.SetBool("isRunning", false);
            }

            //checks if the player is in the ground and is not crouching to play de jump
            if (Keyboard.current.spaceKey.wasPressedThisFrame && !inAir && !isCrouching)
            {
                Jump();
            }

            //checks if the player want to crouch
            if (Keyboard.current.leftCtrlKey.wasPressedThisFrame && !inAir) 
            {
                Crouch();
                tryingToStandUp = false;
            }
            else if (Keyboard.current.leftCtrlKey.wasReleasedThisFrame)
            {
                //if the player release the croch key trys to stand up
                tryingToStandUp = true;
            }
        }

    }

    //move the position of the player
    Vector3 Move(Vector3 moveDirection)
    {
        transform.position = transform.position + moveDirection * movementSpeed * Time.deltaTime;

        //checks if the player is moving
        if (moveDirection.magnitude != 0)
        {
            //check what sound has to be played depending on the player's condition
            if (!inAir)
            {
             
                if (isCrouching || isRunning)
                {
                    if (isCrouching && isRunning)
                    {
                        soundController.CrouchRunClientRpc();
                    }
                    else if (isRunning)
                    {
                        soundController.RunClientRpc();
                    }
                    else
                    {
                        soundController.CrouchWalkClientRpc();
                    }
                }
                else
                {
                    soundController.WalkClientRpc();
                }
            }
            
            animator.SetBool("isWalking", true);

            //if the player is not grabbing rotate him to de desired direction
            if (!isGrabbing)
            {
                LookAt(moveDirection);
            }

            return moveDirection;
        }
        else
        {
            //if the player is not moving stop all the sounds
            soundController.stopAllClientRpc();
            animator.SetBool("isWalking", false);
            return Vector3.zero;
        }
    }

    //rotate the player
    void LookAt(Vector3 lookDirection)
    {
        Quaternion targetRotation;
        targetRotation = Quaternion.LookRotation(lookDirection); 

        Quaternion newRotation;
        newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
                      rotationSpeed * Time.deltaTime);

        transform.rotation = newRotation;
    }

    //calculate the direction that the player is moving acording with the keys that is pressing
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

    //push up the player like jumping
    void Jump()
    {
        soundController.JumpClientRpc();
        

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        inAir = true;
        animator.SetTrigger("Jump");
    }

    //check if the player is touching the ground
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

    //reduce the player collision size by simulating crouching
    private void Crouch()
    {
        //sounds
        if (!isCrouching)
        {
            if (!sliding)
            {
                soundController.CrouchClientRpc();
            }
            else
            {
                sliding = false;
            }
        }

        //reduction
        bc.size = new Vector3(initSize.x, initSize.y / 2, initSize.z);
        bc.center = new Vector3(initCenter.x, -(bc.size.y) / 2, initCenter.z);
        movementSpeed = crouchSpeed;
        isCrouching = true;
        animator.SetBool("isCrouching", true);
    }

    //the player returns to its normal size
    private void StandUp()
    {
        bc.size = initSize;
        bc.center = initCenter;
        movementSpeed = initialSpeed;
        isCrouching = false;
        animator.SetBool("isCrouching", false);
    }

    //increase the players movement speed up to a limit
    private void Run()
    {
        isRunning = true;
        float max = maxSpeed;
        
        //if the player is crouching the speed is less than normal
        if (isCrouching)
        {
            max = maxSpeed - (initialSpeed-crouchSpeed);
        }

        if (movementSpeed < max)
        {
            movementSpeed += movementSpeed * aceleration * Time.deltaTime;
        }
    }

    //check if the player is crouching or not to set his velocity
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

    //pushes the player forward simulating a slide
    void slide(Vector3 moveDirection)
    {
        sliding = true;
        soundController.SlideClientRpc();
        rb.AddForce(moveDirection * slideForce, ForceMode.Impulse);
        animator.SetTrigger("Slide");
    }

    //change the emotion of the player
    void SadEmotion()
    {
        //change the emotion to sad or normal
        if (!differentEmotion)
        {
            //in sad emotion, the speed of the player is less than normal
            emotion = emotions.SAD;
            differentEmotion = true;
            movementSpeed = initialSpeed - 2;
        }
        else
        {
            //normal state
            emotion = emotions.NORMAL;
            differentEmotion = false;
            movementSpeed = initialSpeed;
        }
    }

    //get current emotion
    public emotions getEmotion()
    {
        return this.emotion;
    }

    //check if the player is colliding with an interactuable object
    private void OnTriggerEnter(Collider other)
    {
        SceneInteractableBehaviour aux = other.gameObject.GetComponent<SceneInteractableBehaviour>();

        if (aux != null && interactableObject == null)
        {
            interactableObject = aux;

            
            interactableObject.ActivateEmission();
        }
    }

    //check if the player stops colliding with an interactuable object
    private void OnTriggerExit(Collider other)
    {
        SceneInteractableBehaviour aux = other.gameObject.GetComponent<SceneInteractableBehaviour>();

        if (aux != null && aux == interactableObject)
        {
            //stop grabbing
            if (isGrabbing)
            {
                isGrabbing = false;
                movementSpeed = initialSpeed;
            }

            //stop sounds
            interactableObject.stopSoundClientRpc();

            //clear interactuable object
            interactableObject.DeActivateEmission();
            interactableObject.ClearParent();
            

            interactableObject = null;
        }
    }
  


    //play the push animation that fits with the player movement
    InteractionDirection CalculateInteractionDirection(Vector3 inputDir,Transform objectTransform)
    {
        if (inputDir == Vector3.zero)
        {
            return InteractionDirection.None;
        }
             
        Vector3 localDir = objectTransform.InverseTransformDirection(inputDir);

        float forward = localDir.z;
        float right = localDir.x;

        if (forward > 0.5f)
        {
            return InteractionDirection.PushForward;
        }
            

        if (forward < -0.5f)
        {
            return InteractionDirection.PullBack;
        }
            

        if (right > 0.5f)
        {
            return InteractionDirection.MoveRight;
        }
            

        if (right < -0.5f)
        {
            return InteractionDirection.MoveLeft;
        }
        return InteractionDirection.None;
    }

    //Try to stand up if there is no object above the player
    public void TryToStandUp()
    {
        Debug.DrawRay(transform.position, Vector3.up * standUpDistance, Color.black);

        if (!Physics.Raycast(transform.position, Vector3.up, standUpDistance, 
            ~0,QueryTriggerInteraction.Ignore))
        {
            StandUp();
            tryingToStandUp = false;
        }
    }
}
