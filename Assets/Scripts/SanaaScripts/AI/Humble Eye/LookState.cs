using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "(S) LookState", menuName = "ScriptableObjects/States/LookState")]
public class LookState : State
{
    [SerializeField] private Vector3 rotationA = new Vector3(0, 207.559799f, 0);
    [SerializeField] private Vector3 rotationB = new Vector3(0, 152.788376f, 0);
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 2f;

    private bool lookingAtA = true;
    private float waitTimer = 0f;

    [SerializeField] private AudioClip turnSound;
    [SerializeField] private float hearDistance = 6f;

    public override State Run(GameObject owner)
    {
        Transform t = owner.transform;

        Vector3 targetRotation = lookingAtA ? rotationA : rotationB;
        Quaternion targetQuat = Quaternion.Euler(targetRotation);

        t.rotation = Quaternion.RotateTowards(
            t.rotation,
            targetQuat,
            speed * Time.deltaTime * 100f
        );

        if (Quaternion.Angle(t.rotation, targetQuat) < 0.5f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                lookingAtA = !lookingAtA;
                waitTimer = 0f;

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    float distance = Vector3.Distance(
                        owner.transform.position,
                        player.transform.position
                    );

                    if (distance <= hearDistance)
                    {
                        AudioSource audio = owner.GetComponent<AudioSource>();
                        if (audio != null && turnSound != null)
                        {
                            audio.PlayOneShot(turnSound);
                        }
                    }
                }
            }
        }

        return base.Run(owner);
    }
}
