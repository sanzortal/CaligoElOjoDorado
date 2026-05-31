using UnityEngine;

[CreateAssetMenu(fileName = "(S) AttackState", menuName = "ScriptableObjects/States/AttackState")]
public class AttackState : State
{
    [SerializeField] string killAnim;
    public override State Run(GameObject owner)
    {
        //look for the player
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        GameObject player = controller.gameObject;

        //if the player is active the enemy kills him
        if (controller.isActiveAndEnabled)
        {
            PlayerDeaths deadPlayer = player.GetComponent<PlayerDeaths>();

            deadPlayer.StartDeathCoroutine(killAnim);
        }

        return base.Run(owner);
    }
}
