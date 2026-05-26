using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 initPos;
    private Vector3 initRot;
    private Rigidbody rb;
    void Start()
    {
        initPos = this.transform.position;
        initRot = this.transform.eulerAngles;
        rb = GetComponent<Rigidbody>();
        
        DeathsController.RegisterOnPlayerDeath(SelfRespawn);
    }
    
    public virtual void SelfRespawn()
    {
        this.transform.position = initPos;
        this.transform.eulerAngles = initRot;
        
        if (rb)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
