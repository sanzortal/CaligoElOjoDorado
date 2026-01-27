using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator animator;

    [SerializeField] KeyCode forwardKey = KeyCode.W;
    [SerializeField] KeyCode backwardKey = KeyCode.S;
    [SerializeField] KeyCode leftKey = KeyCode.A;
    [SerializeField] KeyCode rightKey = KeyCode.D;

    [SerializeField] KeyCode crouchKey = KeyCode.LeftShift;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Walk animation
        bool isMoving =
            Input.GetKey(forwardKey) ||
            Input.GetKey(backwardKey) ||
            Input.GetKey(leftKey) ||
            Input.GetKey(rightKey);
 
        animator.SetBool("isWalking", isMoving);

        //Crouch animation
        bool isCrouching = Input.GetKey(crouchKey);
        animator.SetBool("isCrouching", isCrouching);

        //Jump animation
        if (Input.GetKeyDown(jumpKey))
        {
            animator.SetTrigger("jump");
        }
    }
}
