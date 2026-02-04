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
        // Obtener el movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calcular el offset deseado según el mouse
        offset += new Vector3(mouseX, 0, mouseY) * sensitivity;

        // Limitar el offset máximo relativo al jugador
        offset.x = Mathf.Clamp(offset.x, -maxOffsetX, maxOffsetX);
        offset.z = Mathf.Clamp(offset.z, -maxOffsetZ, maxOffsetZ);

        // Posición objetivo de la cámara
        Vector3 targetPos = player.position + offset;

        // Limitar la posición de la cámara a los límites globales
        targetPos.x = Mathf.Clamp(targetPos.x, xLimits.x, xLimits.y);
        targetPos.z = Mathf.Clamp(targetPos.z, zLimits.x, zLimits.y);

        // Aplicar posición suavizada (opcional)
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);

        // Mantener la cámara mirando al jugador
        transform.LookAt(player.position);
    }
}