using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderMovement : NetworkBehaviour
{
    //checks and values
    private bool onLadder;
    private Rigidbody rb;
    private PlayerController playerController;
    private RigidbodyConstraints rbFirstConstraints;
    private float zLadder;
    private float yRotation;
    private Transform grabPosition;
    private Transform endClimbPosition;
    private bool isInEndCollider;
    [SerializeField] Animator animator;

    [SerializeField] float climbSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onLadder = false;
        rb = this.gameObject.GetComponent<Rigidbody>();
        playerController = this.gameObject.GetComponent<PlayerController>();
        rbFirstConstraints = rb.constraints;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        //check if the player is colliding with the ladder
        if (onLadder)
        {
            //if the player pressed the space key starts climbing the ladder
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                //freeze the player constraints and teleport him to the middle of the ladder
                rb.constraints = RigidbodyConstraints.FreezeAll;
                playerController.enabled = false;

                animator.SetBool("isClimbing", true);

                rb.transform.localPosition = new Vector3(grabPosition.position.x, 
                                             rb.transform.position.y, zLadder);
                this.transform.eulerAngles = new Vector3(rb.rotation.x, yRotation, rb.rotation.z);
            }

            //check if the space key is being pressed
            if (Keyboard.current.spaceKey.isPressed)
            {
                //if the player is going up and collide with the end of the ladder teleport him to the final
                if (Input.GetAxisRaw("Vertical") > 0 && isInEndCollider)
                {
                    this.transform.position = endClimbPosition.position;
                }
                //if the player is going down and is in the ground stop the movement
                else if (Input.GetAxisRaw("Vertical") < 0 && !playerController.InAir())
                {
                    return;
                }

                //if the player is in the ladder and press the movement keys, change his position
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x, 
                                              this.transform.position.y + Input.GetAxisRaw("Vertical") 
                                              * climbSpeed * Time.deltaTime, this.transform.position.z);
                }  
            }
            //if the player stop climbing the ladder restore his constraints
            else
            {
                rb.constraints = rbFirstConstraints;
                playerController.enabled = true;

                animator.SetBool("isClimbing", false);
            }
        }
    }

    //check if the player collides with the ladder and get some values
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            onLadder = true;
            zLadder = collision.gameObject.transform.localPosition.z;
            yRotation = collision.gameObject.transform.eulerAngles.y;
            grabPosition = collision.transform.Find("GrabPosition");
            endClimbPosition = collision.transform.Find("EndClimbPosition");
        }
       
        if (collision.gameObject.name.Equals("ClimbTrigger"))
        {
           isInEndCollider = true;
        }
    }

    //if the player exits the ladder restart all the variables
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            onLadder = false;

            if (!playerController.enabled)
            {
                rb.constraints = rbFirstConstraints;
                playerController.enabled = true;
            }

            animator.SetBool("isClimbing", false);

            endClimbPosition = null;
            grabPosition = null;
            zLadder = 0;
            yRotation = 0;
        }
        
        if (collision.gameObject.name.Equals("ClimbTrigger"))
        {
            isInEndCollider = false;
        }
    }
}
