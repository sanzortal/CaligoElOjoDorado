using UnityEngine;

public class KillHole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerDeaths playerDeaths = other.GetComponent<PlayerDeaths>();
        if (playerDeaths != null)
        {
            playerDeaths.StartDeathCoroutine("None");
        }
    }
}
