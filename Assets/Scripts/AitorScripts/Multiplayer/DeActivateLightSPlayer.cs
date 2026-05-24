using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class DeActivateLightSPlayer : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

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
