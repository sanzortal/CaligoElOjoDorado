using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "(S) PatrolState", menuName = "ScriptableObjects/States/PatrolState")]
public class PatrolState : State
{
    [SerializeField] private Vector3[] position;
    private int index = 0;

    public override State Run(GameObject owner)
    {
        NavMeshAgent agentCmp = owner.GetComponent<NavMeshAgent>();

        if (!agentCmp.enabled)
        {
            agentCmp.enabled = true;
        }

        if (agentCmp.remainingDistance <= agentCmp.stoppingDistance)
        {
            index++;

            index %= position.Length;
            agentCmp.SetDestination(position[index]);
        }

        return base.Run(owner);
    }
}
