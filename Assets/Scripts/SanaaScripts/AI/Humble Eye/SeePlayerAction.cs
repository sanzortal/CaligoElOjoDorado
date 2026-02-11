using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "(a) SeePlayerAction", menuName = "ScriptableObjects/Actions/SeePlayerAction")]
public class SeePlayerAction : DrawableAction
{
    [SerializeField] private float visionAngle;
    [SerializeField] private float radiusSphere;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    public override bool Check(GameObject owner)
    {
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        if (controller == null) return false;

        Vector3 toPlayer = controller.transform.position - owner.transform.position;
        float distance = toPlayer.magnitude;

         if (distance > radiusSphere)
            return false;

        Vector3 dirToPlayer = toPlayer.normalized;
        
        float angle = Vector3.Angle(owner.transform.forward, dirToPlayer);
        if (angle > visionAngle * 0.5f)
            return false;

        if (Physics.Raycast(owner.transform.position, dirToPlayer, out RaycastHit hit, distance, obstacleMask | playerMask))
        {
            if (!hit.collider.CompareTag("Player"))
                return false;
        }
        return true;
    }

    public override void DrawGizmo(GameObject owner)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(owner.transform.position, radiusSphere);

        Vector3 left = Quaternion.Euler(0f, -visionAngle / 2, 0f) * owner.transform.forward * radiusSphere;
        Vector3 right = Quaternion.Euler(0f, visionAngle / 2, 0f) * owner.transform.forward * radiusSphere;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(owner.transform.position, left);
        Gizmos.DrawRay(owner.transform.position, right);
    }
}
