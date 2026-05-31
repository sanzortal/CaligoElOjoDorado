using System;
using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //radius of view
    public float radius;

    //angle of view
    [Range(0,360)]
    public float angle;

    public GameObject player;

    //check masks
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    //check player
    public bool canSeePlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FieldOfVisionRoutine());
    }

    //start the movement of the eye waiting for some time
    private IEnumerator FieldOfVisionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return null;
            FieldOfVisionCheck();
        }
    }

    //check if the eye sees the player
    private void FieldOfVisionCheck()
    {
        //range of vision
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        //if is seeing something starts the checks
        if(rangeChecks.Length != 0 )
        {
            //calculate the direction to the player
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //check if the player is in the range of vision of the eye
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //if the player is not hiding the eye kills him
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, 
                                    obstructionMask))
                {
                    canSeePlayer = true;
                    PlayerDeaths deadPlayer= player.GetComponent<PlayerDeaths>();
                    PlayerController controller = player.GetComponent<PlayerController>();

                    //kill
                    if (deadPlayer != null && controller.isActiveAndEnabled)
                    {
                        StartCoroutine(deadPlayer.die("humbleEyeDead"));
                        canSeePlayer = false;
                    }
                }
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
