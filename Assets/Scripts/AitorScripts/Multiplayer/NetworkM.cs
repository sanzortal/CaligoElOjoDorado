using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkM : NetworkBehaviour
{
    [SerializeField] GameObject hugoPrefab;
    [SerializeField] GameObject eyePrefab;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer == false)
            return;
        NetworkManager.OnClientConnectedCallback += ChangeScene;
        NetworkManager.OnClientDisconnectCallback += DisconnectClient;
        NetworkManager.SceneManager.OnLoadEventCompleted += HandleSceneLoadCompleted;
       
    }

    private void HandleSceneLoadCompleted(string sceneName,
        LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in clientsCompleted)
        {
            SpawnPlayer(clientId);
        }
    }

    private void ChangeScene(ulong clientId)
    {
        if (IsServer == false || clientId == NetworkManager.ServerClientId) return;

        Debug.Log("Cliente conectado: " + clientId);
        NetworkManager.Singleton.SceneManager.LoadScene("1_RoomScene", LoadSceneMode.Single);
    }

    private void SpawnPlayer(ulong clientID)
    {
        if (!NetworkManager.ConnectedClients.ContainsKey(clientID))
            return;

        if (NetworkManager.ConnectedClients[clientID].PlayerObject != null)
            return;

        GameObject player;
        if (clientID == NetworkManager.ServerClientId)
        {
            player = Instantiate(hugoPrefab);
        }
        else
        {
            player = Instantiate(eyePrefab);
        }

        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback -= ChangeScene;
            NetworkManager.SceneManager.OnLoadEventCompleted -= HandleSceneLoadCompleted;
            
        }

        base.OnNetworkDespawn();
    }

    private void DisconnectClient(ulong clientId)
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);

        if (IsServer)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }


    public void BackHost()
    {
        if (NetworkManager.Singleton != null && IsServer)
        {
            NetworkManager.Singleton.Shutdown();

        }
    }

    public void PlayAlone()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("1_RoomScene", LoadSceneMode.Single);
    }


}
