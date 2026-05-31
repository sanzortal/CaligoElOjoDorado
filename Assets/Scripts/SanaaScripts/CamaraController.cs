using UnityEngine;
using UnityEngine.UIElements;

public class CamaraController : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Transform player;

    [Header("Distancia máxima del mouse desde el jugador")]
    public float maxOffsetX = 5f;
    public float maxOffsetZ = 3f;

    [Header("Sensibilidad del movimiento de la cámara")]
    public float sensitivity = 0.1f;

    [Header("Límites globales del mapa")]
    public Vector2 xLimits = new Vector2(-10f, 10f);
    public Vector2 zLimits = new Vector2(-10f, 10f);

    private Vector3 offset;

    void Update()
    {
        //get the mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //calculate the desired offset based on the mouse movement
        offset += new Vector3(mouseX, 0, mouseY) * sensitivity;

        //limit the maximum relative offset to the player
        offset.x = Mathf.Clamp(offset.x, -maxOffsetX, maxOffsetX);
        offset.z = Mathf.Clamp(offset.z, -maxOffsetZ, maxOffsetZ);

        //target camera position
        Vector3 targetPos = player.position + offset;

        //limit the camera position to the global limits
        targetPos.x = Mathf.Clamp(targetPos.x, xLimits.x, xLimits.y);
        targetPos.z = Mathf.Clamp(targetPos.z, zLimits.x, zLimits.y);

        //apply smoothed positioning 
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);

        //keep the camera looking at the player
        transform.LookAt(player.position);
    }
}