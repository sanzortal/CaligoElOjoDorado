using UnityEngine;

[CreateAssetMenu(fileName = "(S) AttackState", menuName = "ScriptableObjects/States/AttackState")]
public class AttackState : State
{
    [SerializeField] string killAnim;
    public override State Run(GameObject owner)
    {
        GameObject player = FindFirstObjectByType<PlayerController>().gameObject;

        PlayerDeaths deadPlayer = player.GetComponent<PlayerDeaths>();

        deadPlayer.StartDeathCoroutine(killAnim);
        

        return base.Run(owner);
    }
}
