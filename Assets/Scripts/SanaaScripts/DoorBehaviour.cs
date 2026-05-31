using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    //checks
    [SerializeField] PlayerController.emotions emotionNeeded;
    [SerializeField] bool isMovable;

    private Rigidbody rb;
    private bool isDragging;
    private Transform player;
    private Vector3 offset;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //if the player is in the correct emotion starts moving the door
    public void StartDrag(Transform playerTransform, PlayerController.emotions emotion)
    {
        if (emotion != emotionNeeded || !isMovable) return;

        player = playerTransform;
        isDragging = true;

        //keep the real offset
        offset = transform.position - player.position;

        //is in the ground
        rb.useGravity = true; 
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    //set that the player is no longer moving the door
    public void StopDrag()
    {
        isDragging = false;
        player = null;

        rb.constraints = RigidbodyConstraints.None;
    }

    //move the door position if the player is grabbing it
    private void FixedUpdate()
    {
        if (!isDragging || player == null) return;

        Vector3 targetPos = player.position + offset;

        rb.MovePosition(targetPos);
    }
}
