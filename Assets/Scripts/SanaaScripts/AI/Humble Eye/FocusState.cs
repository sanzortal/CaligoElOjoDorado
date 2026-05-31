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
        //check if is being respawned
        RespawnAirEyes respawnedObject = owner.GetComponent<RespawnAirEyes>();
        if (respawnedObject!= null && respawnedObject.GetRespawned())
        {
            return base.Run(owner);
        }

        //keep variables
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        GameObject player = controller.gameObject;

        Transform ownerT = owner.transform;
        Transform playerT = controller.transform;

        //stop the patrol
        if (isIn3D)
        {
            owner.GetComponent<NavMeshAgent>().SetDestination(owner.transform.position);
        }

        // focus the player
        Vector3 dirToPlayer = playerT.position - ownerT.position;

        if (!isIn3D)
        {
            dirToPlayer.y = 0f;
        }

        //rotate towards the player
        if (dirToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dirToPlayer);

            ownerT.rotation = Quaternion.RotateTowards(
                ownerT.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime * 100f
            );
        }


        //when its already looking at the player, execute the kill.
        if (Quaternion.Angle(ownerT.rotation, Quaternion.LookRotation(dirToPlayer)) < 2f)
        {
            PlayerDeaths deadPlayer = controller.GetComponent<PlayerDeaths>();
            if (deadPlayer != null && controller.isActiveAndEnabled)
            {
                deadPlayer.StartDeathCoroutine(killAnim);

                //respawn
                if (respawnedObject != null)
                {
                    respawnedObject.SetRespawned(true);
                }
            }
        }

        return base.Run(owner);
    }
}
