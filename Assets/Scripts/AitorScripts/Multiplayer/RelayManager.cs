using System.Threading.Tasks;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Unity.Netcode;


public class RelayManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI codeText;
    [SerializeField] TMP_InputField codeArea;
    private async void Start()
    {
        //initialize the multiplayer services
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }

        //sing in the multiplayer services as an anonymous user
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    //start the host and show the code
    public async void StartRelay()
    {
        string joinCode = await StartHostWithRelay();
        codeText.text = joinCode;
    }

    //client join
    public async void JoinRelay()
    {
        await StartClientWithRelay(codeArea.text);
    }

    //create the allocation and start the server as host
    private async Task<string> StartHostWithRelay(int maxConnections = 2)
    {
        Allocation allocation;

        //try to create the allocation
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        }
        catch
        {
            Debug.LogError("Creating allocation failed");
            throw;
        }

        NetworkManager.Singleton.GetComponent<UnityTransport>().
                       SetRelayServerData(new RelayServerData(allocation, "dtls"));

        //get the join code
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    ////create the allocation and start as client
    private async Task<bool> StartClientWithRelay(string joinCode)
    {
        //try to create the allocation if the code is correct
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().
                           SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
        }
        catch
        {
            Debug.Log("Incorrect code");
            return false;
        }
    }

}
