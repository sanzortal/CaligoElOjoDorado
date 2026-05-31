
using UnityEngine;

[CreateAssetMenu(fileName = "(a) HearAction", menuName = "ScriptableObjects/Actions/HearAction")]
public class HearAction : DrawableAction
{
    [SerializeField] private float radius;
    [SerializeField] bool hearAttack;
    private bool screamWasPlayed = false;

    //is chasing the player or not
    [SerializeField] bool isChasing;

    //check if the ear can hear the player. If so, make a sound and start chasing him.
    public override bool Check(GameObject owner)
    {
        Collider[] hits = Physics.OverlapSphere(owner.transform.position, radius);

        foreach (Collider hit in hits) 
        {
            PlayerController controller = hit.GetComponent<PlayerController>();
            
            if (controller)
            {
                //check if the ear is chasing the player
                if (isChasing)
                {
                    return true;
                }

                //check if the player is in the correct emotion to be safe
                if (controller.getEmotion() != PlayerController.emotions.SAD)
                {
                    //play the sound
                    AudioSource scream = owner.GetComponent<AudioSource>();
                    if (!hearAttack && !scream.isPlaying && !screamWasPlayed)
                    {
                        owner.GetComponent<EnemiesSoundController>().playScreamClientRpc();
                        screamWasPlayed = true;
                    }

                    return true;
                }
            }
        }

        //restart sound
        screamWasPlayed = false;
        return false;
    }

    //draw hear radius
    public override void DrawGizmo(GameObject owner)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(owner.transform.position, radius);
    }
}
