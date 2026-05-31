using UnityEngine;
using UnityEngine.AI;

public class RemoveEyeRotation : MonoBehaviour
{
    NavMeshAgent agent;

    //disable the function that makes the eye rotate all the time
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
}
