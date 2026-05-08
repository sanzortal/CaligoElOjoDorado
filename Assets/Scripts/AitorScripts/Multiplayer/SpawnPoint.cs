using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static Transform respawn;

    private void Awake()
    {
        respawn = this.transform;
    }
}
