using System;
using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;

    [Range(0,360)]
    public float angle;

    public GameObject player;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FieldOfVisionRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FieldOfVisionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return null;
            FieldOfVisionCheck();
        }
    }

    private void FieldOfVisionCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0 )
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    PlayerDeaths deadPlayer= player.GetComponent<PlayerDeaths>();

                    if (deadPlayer != null)
                    {
                        StartCoroutine(deadPlayer.die("humbleEyeDead"));
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
