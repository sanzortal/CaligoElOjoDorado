using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "(S) ChaseState", menuName = "ScriptableObjects/States/ChaseState")]

public class ChaseState : State
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    public override State Run(GameObject owner)
    {
        //it can be upgraded
        GameObject player = FindFirstObjectByType<PlayerController>().gameObject;

        NavMeshAgent agentCmp = owner.GetComponent<NavMeshAgent>();

        if (agentCmp.enabled)
        {
            agentCmp.enabled = false;
        }

        Vector3 dirToPlayer = (player.transform.position - owner.transform.position).normalized;

        owner.transform.position = owner.transform.position + dirToPlayer * movementSpeed * Time.deltaTime;

        Quaternion targetRotation;
        targetRotation = Quaternion.LookRotation(dirToPlayer); 

        Quaternion newRotation;
        newRotation = Quaternion.RotateTowards(owner.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        owner.transform.rotation = newRotation;

        return base.Run(owner);
    }
}
