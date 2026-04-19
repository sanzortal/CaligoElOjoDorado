using System.Collections;
using UnityEngine;

public class RespawnAirEyes : Respawn
{

    private bool respawned = false;

    public override void SelfRespawn()
    {
        base.SelfRespawn();

        StartCoroutine(waitRespawn());
    }

    //wait 0.5 seconds to let the eye change to the patrol state before change its rotation
    IEnumerator waitRespawn()
    {
        yield return new WaitForSeconds(0.5f);

        respawned = false;
    }
    public bool GetRespawned()
    {
        return respawned;
    }

    public void SetRespawned(bool respawned)
    {
        this.respawned = respawned;
    }
}
