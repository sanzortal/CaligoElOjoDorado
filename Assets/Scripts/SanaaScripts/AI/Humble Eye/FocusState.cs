using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName ="(S) FocusState", menuName = "ScriptableObjects/States/FocusState")]
public class FocusState : State
{
    [SerializeField] string killAnim;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] bool isIn3D;
    public override State Run(GameObject owner)
    {
        RespawnAirEyes respawnedObject = owner.GetComponent<RespawnAirEyes>();
        if (respawnedObject!= null && respawnedObject.GetRespawned())
        {
            return base.Run(owner);
        }

        PlayerController controller = FindFirstObjectByType<PlayerController>();
        GameObject player = controller.gameObject;

        Transform ownerT = owner.transform;
        Transform playerT = controller.transform;

        //parar de patrullar
        if (isIn3D)
        {
            owner.GetComponent<NavMeshAgent>().SetDestination(owner.transform.position);
        }

        // centrar la mirada al jugador
        Vector3 dirToPlayer = playerT.position - ownerT.position;

        if (!isIn3D)
        {
            dirToPlayer.y = 0f;
        }

        if (dirToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dirToPlayer);

            ownerT.rotation = Quaternion.RotateTowards(
                ownerT.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime * 100f
            );
        }


        // Cuando ya está mirando al jugador, ejecutar la muerte
        if (Quaternion.Angle(ownerT.rotation, Quaternion.LookRotation(dirToPlayer)) < 2f)
        {
            PlayerDeaths deadPlayer = controller.GetComponent<PlayerDeaths>();
            if (deadPlayer != null && controller.isActiveAndEnabled)
            {
                deadPlayer.StartDeathCoroutine(killAnim);

                if (respawnedObject != null)
                {
                    respawnedObject.SetRespawned(true);
                }
            }
        }

        return base.Run(owner);
    }
}
