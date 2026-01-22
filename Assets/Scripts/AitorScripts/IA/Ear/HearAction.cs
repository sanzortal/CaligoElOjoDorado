
using UnityEngine;

[CreateAssetMenu(fileName = "(a) HearAction", menuName = "ScriptableObjects/Actions/HearAction")]
public class HearAction : DrawableAction
{
    [SerializeField] private float radius;
    public override bool Check(GameObject owner)
    {
        Collider[] hits = Physics.OverlapSphere(owner.transform.position, radius);

        foreach (Collider hit in hits) 
        {
            PlayerController controller = hit.GetComponent<PlayerController>();
            if (controller && controller.getEmotion() != PlayerController.emotions.SAD)
            {
                return true;
            }
        }

        return false;
    }

    public override void DrawGizmo(GameObject owner)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(owner.transform.position, radius);
    }
}
