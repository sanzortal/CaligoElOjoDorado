using UnityEngine;

public class KillHole : MonoBehaviour
{
    //play the death of the player without any animation 
    private void OnTriggerEnter(Collider other)
    {
        PlayerDeaths playerDeaths = other.GetComponent<PlayerDeaths>();
        if (playerDeaths != null)
        {
            playerDeaths.StartDeathCoroutine("None");
        }
    }
}
