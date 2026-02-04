using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
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

    public void StartDrag(Transform playerTransform, PlayerController.emotions emotion)
    {
        if (emotion != emotionNeeded || !isMovable) return;

        player = playerTransform;
        isDragging = true;

        // Guardamos el offset REAL
        offset = transform.position - player.position;

        rb.useGravity = true; // sigue en el suelo
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void StopDrag()
    {
        isDragging = false;
        player = null;

        rb.constraints = RigidbodyConstraints.None;
    }

    private void FixedUpdate()
    {
        if (!isDragging || player == null) return;

        Vector3 targetPos = player.position + offset;

        rb.MovePosition(targetPos);
    }
}
