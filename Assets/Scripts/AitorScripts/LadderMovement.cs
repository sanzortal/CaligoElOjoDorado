 using UnityEngine;
using UnityEngine.InputSystem;

public class LadderMovement : MonoBehaviour
{
    private bool onLadder;
    private Rigidbody rb;
    private PlayerController playerController;
    private RigidbodyConstraints rbFirstConstraints;
    private float zLadder;
    private Transform grabPosition;

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
        if (onLadder)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                playerController.enabled = false;
                rb.transform.localPosition = new Vector3(grabPosition.position.x, rb.transform.position.y, zLadder);
            }

            if (Keyboard.current.spaceKey.isPressed)
            {  
                    if (Input.GetAxisRaw("Vertical") < 0 && !playerController.InAir())
                    {
                        return;
                    }
                    else
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Input.GetAxisRaw("Vertical") * climbSpeed * Time.deltaTime, this.transform.position.z);    
            }
            else
            {
                rb.constraints = rbFirstConstraints;
                playerController.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            onLadder = true;
            zLadder = collision.gameObject.transform.localPosition.z;
            grabPosition = collision.transform.Find("GrabPosition");
        }
    }

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

            grabPosition = null;
            zLadder = 0;

        }
    }
}
