using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "(S) LookState", menuName = "ScriptableObjects/States/LookState")]
public class LookState : State
{
    //max rotations
    [SerializeField] private Vector3 rotationA = new Vector3(0, 207.559799f, 0);
    [SerializeField] private Vector3 rotationB = new Vector3(0, 152.788376f, 0);

    //speed and wait time
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 2f;

    //checks
    private bool lookingAtA = true;
    private float waitTimer = 0f;

    //range of hear
    [SerializeField] private float hearDistance = 15f;

    public override State Run(GameObject owner)
    {
        Transform t = owner.transform;

        //rotate to one of the two max rotations
        Vector3 targetRotation = lookingAtA ? rotationA : rotationB;
        Quaternion targetQuat = Quaternion.Euler(targetRotation);

        t.rotation = Quaternion.RotateTowards(
            t.rotation,
            targetQuat,
            speed * Time.deltaTime * 100f
        );

        //check if is rotated
        if (Quaternion.Angle(t.rotation, targetQuat) < 0.5f)
        {
            waitTimer += Time.deltaTime;

            //if enough time has passed 
            if (waitTimer >= waitTime)
            {
                lookingAtA = !lookingAtA;
                waitTimer = 0f;

                //check if the player is in the hear range of the eye
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    float distance = Vector3.Distance(
                        owner.transform.position,
                        player.transform.position
                    );

                    //if the player is close enough play the sound
                    if (distance <= hearDistance)
                    {
                        AudioSource audio = owner.GetComponent<AudioSource>();
                        if (audio != null )
                        {
                            audio.Play();
                        }
                    }
                }
            }
        }

        return base.Run(owner);
    }
}
