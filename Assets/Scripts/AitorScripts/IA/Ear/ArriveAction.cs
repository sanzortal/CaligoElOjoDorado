using NUnit.Framework.Constraints;
using UnityEngine;

[CreateAssetMenu(fileName = "(a) ArriveAction", menuName = "ScriptableObjects/Actions/ArriveAction")]

public class ArriveAction : DrawableAction
{
    //checks
    [SerializeField] Vector3 recoverPoint;
    [SerializeField] float tolerance;

    //check if the enemy is inside of the recover point or not
    public override bool Check(GameObject owner)
    {
        return Vector3.Distance(owner.transform.position, recoverPoint) <= tolerance;
    }

    //draw the recover point
    public override void DrawGizmo(GameObject owner)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(recoverPoint,2);
    }
}
