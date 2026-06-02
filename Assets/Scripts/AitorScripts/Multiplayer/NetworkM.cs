using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkM : NetworkBehaviour
{
    //players prefabs
    [SerializeField] GameObject hugoPrefab;
    [SerializeField] GameObject eyePrefab;
    private bool backButton;

    //set unique
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        backButton = false;
    }
    
    //when this object spawn, register all the functions
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.OnClientDisconnectCallback += DisconnectClient;
        if (IsServer == false)
            return;
        
        NetworkManager.OnClientConnectedCallback += ChangeScene;
        NetworkManager.SceneManager.OnLoadEventCompleted += HandleSceneLoadCompleted;
       
    }

    //when a scene is loaded, spawn all the players
    private void HandleSceneLoadCompleted(string sceneName,
        LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        if (!IsServer || SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name == "FinalScreen") return;
        foreach (ulong clientId in clientsCompleted)
        {
            SpawnPlayer(clientId);
        }
    }

    //change the current scene to the first level
    private void ChangeScene(ulong clientId)
    {
        if (IsServer == false || clientId == NetworkManager.ServerClientId) return;

        Debug.Log("Cliente conectado: " + clientId);
        NetworkManager.Singleton.SceneManager.LoadScene("1_RoomScene", LoadSceneMode.Single);
    }

    //check the id of the player and according to that spawn one prefab or another
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

    //unregister functions
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback -= ChangeScene;
            NetworkManager.SceneManager.OnLoadEventCompleted -= HandleSceneLoadCompleted;
            
        }
         

        base.OnNetworkDespawn();
    }

    //when one player closes the app, this function close the app of all players
    private void DisconnectClient(ulong clientId)
    {
        if (!backButton)
        {
            Application.Quit();
        }
    }

    //if the player click back in the main menu, shut down the network manager and change the panel
    public void BackHost()
    {
        if (NetworkManager.Singleton != null && IsServer)
        {
            NetworkManager.Singleton.Shutdown();
            backButton = true;
        }
    }

    //if the player clicks the play alone button, change the scene to the first level
    public void PlayAlone()
    {
        if (NetworkManager.Singleton.SceneManager == null) return;
        NetworkManager.Singleton.SceneManager.LoadScene("1_RoomScene", LoadSceneMode.Single);
    }


}
