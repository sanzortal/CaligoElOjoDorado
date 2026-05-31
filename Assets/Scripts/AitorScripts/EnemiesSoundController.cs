using Unity.Netcode;
using UnityEngine;

public class EnemiesSoundController : NetworkBehaviour
{
    private AudioSource audioScream;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioScream = GetComponent<AudioSource>();
    }

    //play the scream sound also for all the clients
    [ClientRpc]
    public void playScreamClientRpc()
    {
        audioScream.Play();
    }
}
