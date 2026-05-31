using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSoundController : NetworkBehaviour
{
    //sounds that are played
    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource crouch;
    [SerializeField] AudioSource crouchWalk;
    [SerializeField] AudioSource crouchRun;
    [SerializeField] AudioSource slide;
    [SerializeField] AudioSource run;
    private AudioSource[] allAudios = new AudioSource[4];

    //audios that cant be stopped
    private AudioSource audioToNoStop;

    //audios that can be stopped
    private void Awake()
    {
        allAudios[0] = walk;
        allAudios[1] = run;
        allAudios[2] = crouchWalk;
        allAudios[3] = crouchRun;
    }

    /// <summary>
    /// Play all the audios also for the clients checking if they are playing or not
    /// </summary>

    [ClientRpc]
    public void WalkClientRpc()
    {
        stopAll(walk);
        if (!walk.isPlaying)
        {
            walk.Play();
        }
    }

    [ClientRpc]
    public void JumpClientRpc()
    {
        stopAllClientRpc();
        jump.Play();
    }

    [ClientRpc]
    public void RunClientRpc()
    {
        stopAll(run);
        if (!run.isPlaying)
        {
            run.Play();
        }
    }

    [ClientRpc]
    public void CrouchClientRpc()
    {
        stopAllClientRpc();
        crouch.Play();
    }

    [ClientRpc]
    public void CrouchWalkClientRpc()
    {
        stopAll(crouchWalk);
        if (!crouchWalk.isPlaying)
        {
            crouchWalk.Play();
        }
    }

    [ClientRpc]
    public void CrouchRunClientRpc()
    {
        stopAll(crouchRun);
        if (!crouchRun.isPlaying)
        {
            crouchRun.Play();
        }
    }

    [ClientRpc]
    public void SlideClientRpc()
    {
        stopAllClientRpc();
        slide.Play();
    }

    //stop all the audios
    public void stopAll(AudioSource au)
    {
        audioToNoStop = au;
        stopAlmostAllClientRpc();
    }


    //stop all the audios except the one sent as a parameter
    [ClientRpc]
    private void stopAlmostAllClientRpc()
    {
        foreach (AudioSource audio in allAudios)
        {
            if (audio != audioToNoStop)
            {
                audio.Stop();
            }
        }
    }

    //stop all the audios also for the clients
    [ClientRpc]
    public void stopAllClientRpc()
    {
        foreach (AudioSource audio in allAudios)
        {
            audio.Stop();   
        }
    }
}
