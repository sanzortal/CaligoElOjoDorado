
using UnityEngine;

[CreateAssetMenu(fileName = "(a) HearAction", menuName = "ScriptableObjects/Actions/HearAction")]
public class HearAction : DrawableAction
{
    [SerializeField] private float radius;
    [SerializeField] bool hearAttack;
    private bool screamWasPlayed = false;

    //is chasing the player or not
    [SerializeField] bool isChasing;
    public override bool Check(GameObject owner)
    {
        Collider[] hits = Physics.OverlapSphere(owner.transform.position, radius);

        foreach (Collider hit in hits) 
        {
            PlayerController controller = hit.GetComponent<PlayerController>();
            
            if (controller)
            {
                if (isChasing)
                {
                    return true;
                }

                if (controller.getEmotion() != PlayerController.emotions.SAD)
                {
                    AudioSource scream = owner.GetComponent<AudioSource>();
                    if (!hearAttack && !scream.isPlaying && !screamWasPlayed)
                    {
                        owner.GetComponent<AudioSource>().Play();
                        screamWasPlayed = true;
                    }
                    else
                    {
                        //@TO DO en el futuro sonido de atacar
                    }

                    return true;
                }
            }
        }

        screamWasPlayed = false;
        return false;
    }

    public override void DrawGizmo(GameObject owner)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(owner.transform.position, radius);
    }
}
