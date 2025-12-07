using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeaths : MonoBehaviour
{
    [SerializeField] DeathsController dc;
    private Rigidbody rb;
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        playerController = this.gameObject.GetComponent<PlayerController>();
    }
    public IEnumerator die(string enemyKiller)
    {
        //stop player
        playerController.enabled = false;

        //animacion de morir con animation.play(enemykiller) al tener diferentes animaciones

        //turn off camera
        Transform respawn = dc.ActivatePanel();

        //respawn player
        this.transform.position = respawn.position;
        this.transform.eulerAngles = respawn.eulerAngles;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        //wait
        yield return new WaitForSeconds(3);
        //turn on camera
        dc.DeactivatePanel();

        //player movement
        playerController.enabled = true;

    }
}
