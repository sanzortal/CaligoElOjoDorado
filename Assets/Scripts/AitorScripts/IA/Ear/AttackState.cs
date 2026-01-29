using UnityEngine;

[CreateAssetMenu(fileName = "(S) AttackState", menuName = "ScriptableObjects/States/AttackState")]
public class AttackState : State
{
    [SerializeField] string killAnim;
    public override State Run(GameObject owner)
    {
        PlayerController controller = FindFirstObjectByType<PlayerController>();
        GameObject player = controller.gameObject;

        if (controller.isActiveAndEnabled)
        {
            PlayerDeaths deadPlayer = player.GetComponent<PlayerDeaths>();

            deadPlayer.StartDeathCoroutine(killAnim);
        }

        return base.Run(owner);
    }
}
