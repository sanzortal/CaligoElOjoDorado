using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource crouch;
    [SerializeField] AudioSource crouchWalk;
    [SerializeField] AudioSource crouchRun;
    [SerializeField] AudioSource slide;
    [SerializeField] AudioSource run;
    private AudioSource[] allAudios = new AudioSource[4];

    private void Start()
    {
        allAudios[0] = walk;
        allAudios[1] = run;
        allAudios[2] = crouchWalk;
        allAudios[3] = crouchRun;
    }

    public void Walk()
    {
        stopAll(walk);
        if (!walk.isPlaying)
        {
            walk.Play();
        }
    }

    public void Jump()
    {
        stopAll();
        jump.Play();
    }

    public void Run()
    {
        stopAll(run);
        if (!run.isPlaying)
        {
            run.Play();
        }
    }

    public void Crouch()
    {
        stopAll();
        crouch.Play();
    }

    public void CrouchWalk()
    {
        stopAll(crouchWalk);
        if (!crouchWalk.isPlaying)
        {
            crouchWalk.Play();
        }
    }

    public void CrouchRun()
    {
        stopAll(crouchRun);
        if (!crouchRun.isPlaying)
        {
            crouchRun.Play();
        }
    }

    public void Slide()
    {
        stopAll();
        slide.Play();
    }



    public void stopAll(AudioSource au)
    {
        foreach (AudioSource audio in allAudios)
        {
            if (audio != au)
            {
                audio.Stop();
            }
        }
    }

    public void stopAll()
    {
        foreach (AudioSource audio in allAudios)
        {
            audio.Stop();   
        }
    }
}
