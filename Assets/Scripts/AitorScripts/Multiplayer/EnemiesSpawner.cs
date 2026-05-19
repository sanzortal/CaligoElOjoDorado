using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesSpawner : NetworkBehaviour
{
    [SerializeField] GameObject[] enemies;

    private void Start()
    {
     
        if (!IsServer) return;
        StartCoroutine(WaitSpawn());
    }

    IEnumerator WaitSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        ActivateEnemies();
       
    }
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
