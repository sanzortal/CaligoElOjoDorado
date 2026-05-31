using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "(S) PatrolState", menuName = "ScriptableObjects/States/PatrolState")]
public class PatrolState : State
{
    //patrol points
    [SerializeField] private Vector3[] position;
    private int index = 0;

    public override State Run(GameObject owner)
    {
        //move the enemy from one point to another simulating a patrol
        NavMeshAgent agentCmp = owner.GetComponent<NavMeshAgent>();


        if (!agentCmp.enabled)
        {
            agentCmp.enabled = true;
        }

        //set the destination
        if (agentCmp.remainingDistance <= agentCmp.stoppingDistance)
        {
            index++;

            index %= position.Length;
            agentCmp.SetDestination(position[index]);
        }

        return base.Run(owner);
    }
}
