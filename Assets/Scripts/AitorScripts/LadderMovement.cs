 using UnityEngine;
using UnityEngine.InputSystem;

public class LadderMovement : MonoBehaviour
{
    private bool onLadder;
    private Rigidbody rb;
    private PlayerController playerController;
    private RigidbodyConstraints rbFirstConstraints;
    private float zLadder;
    private float xLadder;

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
            if (Keyboard.current.spaceKey.isPressed)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                playerController.enabled = false;
                rb.transform.localPosition = new Vector3(rb.transform.position.x, rb.transform.position.y, zLadder);
               
                    if (Input.GetAxisRaw("Horizontal") < 0 && !playerController.InAir())
                    {
                        return;
                    }
                    else
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Input.GetAxisRaw("Horizontal") * climbSpeed * Time.deltaTime, this.transform.position.z);
                
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
            xLadder = collision.gameObject.transform.localPosition.x;
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

            zLadder = 0;
            xLadder = 0;
        }
    }
}
