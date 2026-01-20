using UnityEngine;

[CreateAssetMenu(fileName = "(S) RecoverPositionState", menuName = "ScriptableObjects/States/RecoverPositionState")]

public class RecoverPositionState : State
{
    [SerializeField] Vector3 recoverPoint;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    public override State Run(GameObject owner)
    {
        Vector3 dirToPoint = (recoverPoint - owner.transform.position).normalized;

        owner.transform.position = owner.transform.position + dirToPoint * movementSpeed * Time.deltaTime;

        Quaternion targetRotation;
        targetRotation = Quaternion.LookRotation(dirToPoint);

        Quaternion newRotation;
        newRotation = Quaternion.RotateTowards(owner.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        owner.transform.rotation = newRotation;
        return base.Run(owner);
    }
}