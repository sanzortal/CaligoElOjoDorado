using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeaths : NetworkBehaviour
{
    private Rigidbody rb;
    private PlayerController playerController;
    private Transform respawn;

    [SerializeField] ParticleSystem fireParticles;
    [SerializeField] Animator animator;
    [SerializeField] float delayBeforeDeathAnim = 2f;

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

        //play particles
        if (fireParticles != null && !enemyKiller.Equals("None"))
        {
            fireParticles.gameObject.SetActive(true);
            fireParticles.Play();
        }

        // wait before play death animation
        //yield return new WaitForSeconds(delayBeforeDeathAnim);

        //animacion de morir con animation.play(enemykiller) al tener diferentes animaciones
        if (!enemyKiller.Equals("None"))
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(3f);
        }

        //turn off camera
        respawn = DeathsController.ActivatePanel();
        
        yield return new WaitForSeconds(1f);
        //Respawn
        Respawn();
        DeathsController.RespawnAll();
        // reset animator
        animator.Rebind();
        animator.Update(0f);

        //stop particles
        if (fireParticles != null)
        {
            fireParticles.Stop();
            fireParticles.gameObject.SetActive(false);
        }

        //wait
        yield return new WaitForSeconds(5.5f);
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
        if (!IsServer) return;

        //respawn player
        this.transform.position = respawn.position;
        this.transform.eulerAngles = respawn.eulerAngles;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        if (NetworkManager.Singleton.ConnectedClients.Count > 1)
        {
            RespawnClients_ClientRpc();
        }
    }

    [ClientRpc]
    private void RespawnClients_ClientRpc()
    {
        SecondPlayerController sp = FindFirstObjectByType<SecondPlayerController>();
        if (sp != null)
        {
            GameObject gsp = sp.gameObject;
            //respawn second player
            gsp.transform.position = respawn.position;
            gsp.transform.eulerAngles = respawn.eulerAngles;
            gsp.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            gsp.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

}
