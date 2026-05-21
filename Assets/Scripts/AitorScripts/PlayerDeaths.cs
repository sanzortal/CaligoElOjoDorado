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

        //animacion de morir con animation.play(enemykiller) al tener diferentes animaciones
        if (!enemyKiller.Equals("None"))
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(3f);
        }

        //turn off camera
        ShowPanelClientRpc();
        respawn = DeathsController.ReturnRespawnPoint();
        
        yield return new WaitForSeconds(1f);
        //Respawn
        Respawn();
        RespawnAllClientRpc();
        // reset animator
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
            RespawnClients_ClientRpc(respawn.position, respawn.eulerAngles);
        }
    }

    [ClientRpc]
    private void RespawnClients_ClientRpc(Vector3 respawnPos, Vector3 respawnRot)
    {
        SecondPlayerController sp = FindFirstObjectByType<SecondPlayerController>();
        if (sp != null)
        {
            GameObject gsp = sp.gameObject;
            //respawn second player
            gsp.transform.position = respawnPos;
            gsp.transform.eulerAngles = respawnRot;
            gsp.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            gsp.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    [ClientRpc]
    private void ShowParticlesClientRpc()
    {
        fireParticles.gameObject.SetActive(true);
        fireParticles.Play();
    }

    [ClientRpc]
    private void HideParticlesClientRpc()
    {
        fireParticles.Stop();
        fireParticles.gameObject.SetActive(false);
    }

    [ClientRpc]
    private void ShowPanelClientRpc()
    {
        DeathsController.ActivatePanel();
    }

    [ClientRpc]
    private void HidePanelClientRpc()
    {
        DeathsController.DeactivatePanel();
    }

    [ClientRpc]
    private void RespawnAllClientRpc()
    {
        DeathsController.RespawnAll();
    }

}
