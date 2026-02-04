using UnityEngine;

[CreateAssetMenu(fileName = "(a) SeePlayerAction", menuName = "ScriptableObjects/Actions/SeePlayerAction")]
public class SeePlayerAction : DrawableAction
{
    [SerializeField] private float visionAngle;
    [SerializeField] private float radiusSphere;

    public override bool Check(GameObject owner)
    {
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        Vector3 dirToPlayer = (controller.transform.position - owner.transform.position).normalized;

        /*
         * if (dirToPlayer.magnitude > radiusSphere)
            return false;
        */

        float angle = Vector3.Angle(owner.transform.forward, dirToPlayer);

        if (angle <= visionAngle / 2)
        {
            return true;
        }

        return false;
    }

    public override void DrawGizmo(GameObject owner)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(owner.transform.position, radiusSphere);

        Vector3 left = Quaternion.Euler(0f, -visionAngle / 2, 0f) * owner.transform.forward * radiusSphere;
        Vector3 right = Quaternion.Euler(0f, visionAngle / 2, 0f) * owner.transform.forward * radiusSphere;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(owner.transform.position, left);
        Gizmos.DrawRay(owner.transform.position, right);
    }
}
