using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkM : NetworkBehaviour
{
    [SerializeField] GameObject hugoPrefab;
    [SerializeField] GameObject eyePrefab;
    private bool backButton;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        backButton = false;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.OnClientDisconnectCallback += DisconnectClient;
        if (IsServer == false)
            return;
        
        NetworkManager.OnClientConnectedCallback += ChangeScene;
        NetworkManager.SceneManager.OnLoadEventCompleted += HandleSceneLoadCompleted;
       
    }

    private void HandleSceneLoadCompleted(string sceneName,
        LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        if (!IsServer || SceneManager.GetActiveScene().name == "Main Menu") return;
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
            player = Instantiate(hugoPrefab, SpawnPoint.respawn.position, Quaternion.identity);
        }
        else
        {
            player = Instantiate(eyePrefab, SpawnPoint.respawn.position, Quaternion.identity);
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
         //NetworkManager.OnClientDisconnectCallback -= DisconnectClient;

        base.OnNetworkDespawn();
    }

    private void DisconnectClient(ulong clientId)
    {
        if (!backButton)
        {
            Application.Quit();
        }
    }



    public void BackHost()
    {
        if (NetworkManager.Singleton != null && IsServer)
        {
            NetworkManager.Singleton.Shutdown();
            backButton = true;
        }
    }

    public void PlayAlone()
    {
        if (NetworkManager.Singleton.SceneManager == null) return;
        NetworkManager.Singleton.SceneManager.LoadScene("1_RoomScene", LoadSceneMode.Single);
    }


}
