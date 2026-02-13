using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeaths : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerController playerController;
    private Transform respawn;

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
        respawn = DeathsController.ActivatePanel();
        
        yield return new WaitForSeconds(1.5f);
        //Respawn
        Respawn();
        DeathsController.RespawnAll();
        //wait
        yield return new WaitForSeconds(2f);
        //turn on camera
        DeathsController.DeactivatePanel();

        //player movement
        playerController.enabled = true;

    }

    public void StartDeathCoroutine(string enemyKiller)
    {
        StartCoroutine(die(enemyKiller));
    }

    public void Respawn()
    {
        //respawn player
        this.transform.position = respawn.position;
        this.transform.eulerAngles = respawn.eulerAngles;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
}
