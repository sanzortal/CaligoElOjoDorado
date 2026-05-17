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
        ActivateEnemiesClientRpc();
       
    }
    [ClientRpc]
    void ActivateEnemiesClientRpc()
    {
        foreach (GameObject g in enemies)
        {
            g.SetActive(true);
        }
    }
}
