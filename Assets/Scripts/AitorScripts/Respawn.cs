using UnityEngine;

public class Respawn : MonoBehaviour
{
    //object values
    private Vector3 initPos;
    private Vector3 initRot;
    private Rigidbody rb;

    //get values and resgister in the death function
    void Start()
    {
        initPos = this.transform.position;
        initRot = this.transform.eulerAngles;
        rb = GetComponent<Rigidbody>();
        
        DeathsController.RegisterOnPlayerDeath(SelfRespawn);
    }

    //set the rotation and the position of this object to what its initial values were
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
