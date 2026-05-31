using Unity.Netcode;
using UnityEngine;

public class DeActivateStateMachine : NetworkBehaviour
{
    //keep the state machine deactivate or activate
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
