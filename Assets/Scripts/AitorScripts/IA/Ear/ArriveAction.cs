using NUnit.Framework.Constraints;
using UnityEngine;

[CreateAssetMenu(fileName = "(a) ArriveAction", menuName = "ScriptableObjects/Actions/ArriveAction")]

public class ArriveAction : DrawableAction
{
    [SerializeField] Vector3 recoverPoint;
    [SerializeField] float tolerance;
    public override bool Check(GameObject owner)
    {
        return Vector3.Distance(owner.transform.position, recoverPoint) <= tolerance;
    }

    public override void DrawGizmo(GameObject owner)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(recoverPoint,2);
    }
}
