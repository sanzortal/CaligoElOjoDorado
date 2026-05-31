using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeaths : NetworkBehaviour
{
    //player variables
    private Rigidbody rb;
    private PlayerController playerController;
    private Transform respawn;

    //particle sistem
    [SerializeField] ParticleSystem fireParticles;
    [SerializeField] Animator animator;
    
    //sound
    private PlayerSoundController soundController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        playerController = this.gameObject.GetComponent<PlayerController>();
        soundController = this.gameObject.GetComponent<PlayerSoundController>();
    }
    public IEnumerator die(string enemyKiller)
    {
        if (!IsServer) yield break;

        //stop player
        playerController.enabled = false;
        soundController.stopAllClientRpc();

        //play particles
        if (fireParticles != null && !enemyKiller.Equals("None"))
        {
            ShowParticlesClientRpc();
        }

        //play die animation
        if (!enemyKiller.Equals("None"))
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(3f);
        }

        //turn off camera
        ShowPanelClientRpc();
        respawn = DeathsController.ReturnRespawnPoint();
        
        yield return new WaitForSeconds(1f);

        //respawn
        Respawn();
        RespawnAllClientRpc();

        //reset animator
        animator.Rebind();
        animator.Update(0f);

        //stop particles
        if (fireParticles != null)
        {
            HideParticlesClientRpc();
        }

        //wait
        yield return new WaitForSeconds(5.5f);

        //turn on camera
        HidePanelClientRpc();

        //activate player movement
        playerController.enabled = true;

    }

    public void StartDeathCoroutine(string enemyKiller)
    {
        StartCoroutine(die(enemyKiller));
    }

    //respawn player with his initial values
    public void Respawn()
    {
        if (!IsServer) return;

        this.transform.position = respawn.position;
        this.transform.eulerAngles = respawn.eulerAngles;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        if (NetworkManager.Singleton.ConnectedClients.Count > 1)
        {
            RespawnClients_ClientRpc(respawn.position, respawn.eulerAngles);
        }
    }

    //respawn second player with his initial values
    [ClientRpc]
    private void RespawnClients_ClientRpc(Vector3 respawnPos, Vector3 respawnRot)
    {
        SecondPlayerController sp = FindFirstObjectByType<SecondPlayerController>();
        if (sp != null)
        {
            GameObject gsp = sp.gameObject;
            
            gsp.transform.position = respawnPos;
            gsp.transform.eulerAngles = respawnRot;
            gsp.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            gsp.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    //show fire particles also for the clients
    [ClientRpc]
    private void ShowParticlesClientRpc()
    {
        fireParticles.gameObject.SetActive(true);
        fireParticles.Play();
    }

    //hide fire particles also for the clients
    [ClientRpc]
    private void HideParticlesClientRpc()
    {
        fireParticles.Stop();
        fireParticles.gameObject.SetActive(false);
    }

    //show the death panel also for the clients
    [ClientRpc]
    private void ShowPanelClientRpc()
    {
        DeathsController.ActivatePanel();
    }

    //hide the death panel also for the clients
    [ClientRpc]
    private void HidePanelClientRpc()
    {
        DeathsController.DeactivatePanel();
    }

    //respawn all the objects also for the clients
    [ClientRpc]
    private void RespawnAllClientRpc()
    {
        DeathsController.RespawnAll();
    }

}
