using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public static Transform respawn;

    //change a static variable that is used to know which is the first spawn point
    private void Awake()
    {
        respawn = this.transform;
    }
}
