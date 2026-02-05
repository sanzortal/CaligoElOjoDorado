using UnityEngine;

[CreateAssetMenu(fileName ="(S) FocusState", menuName = "ScriptableObjects/States/FocusState")]
public class FocusState : State
{
    [SerializeField] string killAnim;
    [SerializeField] private float rotationSpeed = 5f;

    public override State Run(GameObject owner)
    {
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        GameObject player = controller.gameObject;

        Transform ownerT = owner.transform;
        Transform playerT = controller.transform;

        // centrar la mirada al jugador
        Vector3 dirToPlayer = playerT.position - ownerT.position;
        dirToPlayer.y = 0f;

        if (dirToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dirToPlayer);

            ownerT.rotation = Quaternion.RotateTowards(
                ownerT.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime * 100f
            );
        }

        /* viejo 
         * if (controller.isActiveAndEnabled)
        {
            PlayerDeaths deadPlayer = player.GetComponent<PlayerDeaths>();

            deadPlayer.StartDeathCoroutine(killAnim);
        }*/

        // Cuando ya está mirando al jugador, ejecutar la muerte
        if (Quaternion.Angle(ownerT.rotation, Quaternion.LookRotation(dirToPlayer)) < 2f)
        {
            PlayerDeaths deadPlayer = controller.GetComponent<PlayerDeaths>();
            if (deadPlayer != null && controller.isActiveAndEnabled)
            {
                deadPlayer.StartDeathCoroutine(killAnim);
            }
        }

        return base.Run(owner);
    }
}
