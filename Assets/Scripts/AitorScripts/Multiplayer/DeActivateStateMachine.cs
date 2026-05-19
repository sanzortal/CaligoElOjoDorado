using Unity.Netcode;
using UnityEngine;

public class DeActivateStateMachine : NetworkBehaviour
{
    private void Start()
    {
   
        if (!IsServer) return;

        StateMachine stateMachine = GetComponent<StateMachine>();

        if (stateMachine != null)
        {
            stateMachine.enabled = true;
        }
    }
}
