 using UnityEngine;
using UnityEngine.InputSystem;

public class LadderMovement : MonoBehaviour
{
    private bool onLadder;
    private Rigidbody rb;
    private PlayerController playerController;
    private RigidbodyConstraints rbFirstConstraints;

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

                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    if (Input.GetAxisRaw("Horizontal") < 0 && !playerController.InAir())
                    {
                        return;
                    }

                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Input.GetAxisRaw("Horizontal") *climbSpeed*Time.deltaTime, this.transform.position.z);
                }
            }
            else
            {
                rb.constraints = rbFirstConstraints;
                playerController.enabled = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            onLadder = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ladder"))
        {
            onLadder = false;

            if (!playerController.enabled)
            {
                rb.constraints = rbFirstConstraints;
                playerController.enabled = true;
            }
        }
    }
}
