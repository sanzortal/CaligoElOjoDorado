using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesSpawner : NetworkBehaviour
{
    //enemies that have to be synchronized
    [SerializeField] GameObject[] enemies;

    private void Start()
    {
        if (!IsServer) return;
        StartCoroutine(WaitSpawn());
    }

    IEnumerator WaitSpawn()
    {
        //wait x seconds before activate the enemies
        yield return new WaitForSeconds(0.2f);
        ActivateEnemies();
       
    }

    //activate the ia agent and the state machine of the enemies
    void ActivateEnemies()
    {
        foreach (GameObject g in enemies)
        {
            NavMeshAgent agent = g.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                agent.enabled = true;
            }

            StateMachine st = g.GetComponent<StateMachine>();

            if (st != null)
            {
                st.enabled = true;
            }

            
        }
    }
}
