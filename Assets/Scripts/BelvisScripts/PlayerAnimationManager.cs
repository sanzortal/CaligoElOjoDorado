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
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;
    [SerializeField] KeyCode stairsKey = KeyCode.E;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isMoving =
            Input.GetKey(forwardKey) ||
            Input.GetKey(backwardKey) ||
            Input.GetKey(leftKey) ||
            Input.GetKey(rightKey);

        bool isCrouching = Input.GetKey(crouchKey);


        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isCrouching", isCrouching);

        if (Input.GetKeyDown(jumpKey))
        {
            animator.SetTrigger("Jump");
        }

        if (Input.GetKeyDown(slideKey))
        {
            animator.SetTrigger("Slide");
        }
        

    }
}
