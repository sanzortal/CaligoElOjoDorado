using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class DeActivateLightSPlayer : NetworkBehaviour
{
    //when the light spawn, checks if there is 2 players 
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //if there is 2 players deactivates the light
        if (NetworkManager.ConnectedClients.Count>=2)
        {
            Light light = this.GetComponent<Light>();

            if (light != null)
            {
                light.enabled = false;
            }
        }

    }
}
