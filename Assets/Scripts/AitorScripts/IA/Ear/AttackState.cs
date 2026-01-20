using UnityEngine;

[CreateAssetMenu(fileName = "(S) AttackState", menuName = "ScriptableObjects/States/AttackState")]
public class AttackState : State
{
    public override State Run(GameObject owner)
    {
        GameObject player = FindFirstObjectByType<PlayerController>().gameObject;

        PlayerDeaths deadPlayer = player.GetComponent<PlayerDeaths>();

        //StartCoroutine(deadPlayer.die("earDeath"));
        

        return base.Run(owner);
    }
}
